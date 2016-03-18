/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 15/03/2016
 * Time: 14:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;  
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging; 
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using GalaSoft.MvvmLight.Command;
using Microsoft.Xaml.Interactivity;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Credentials;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Logic.Model;
using CredentialsCreator = Tweetinvi.CredentialsCreator;
using Tweet = Tweetinvi.Logic.Tweet;
using User = Tweetinvi.User;
using AppNet.Model;
using GalaSoft.MvvmLight.Command;

namespace AppNet.ViewModels
{
    public class LoggedUserViewModel : ViewModelBase
    {
    	private List<Tweetinvi.Core.Interfaces.DTO.IMedia> MediasTweet;
    	
    	public LoggedUserViewModel()
        {
            this.Auser = (Tweetinvi.Logic.User) User.GetAuthenticatedUser();
            this.TimeLineTweets = getTimeLine(this.Auser);

            this.MediasTweet = new List<Tweetinvi.Core.Interfaces.DTO.IMedia>();
        }
    	
    	private Tweetinvi.Logic.User _Auser;
    	
    	public Tweetinvi.Logic.User Auser
		{
			get{return _Auser; }
            set{ Set(ref _Auser, value);}
            
		}
    	
    	private ObservableCollection<Tweetinvi.Logic.Tweet> _timeLineTweets;

        public ObservableCollection<Tweetinvi.Logic.Tweet> TimeLineTweets
        {
            get { return _timeLineTweets; }
            set
            {
                Set(ref _timeLineTweets, value);
            }
        }
   
        public ObservableCollection<Tweet> getTimeLine(Tweetinvi.Logic.User Auser)
        {
            var timeLine = Timeline.GetUserTimeline(Auser);
            var timeLineListe = new ObservableCollection<Tweetinvi.Logic.Tweet>();
            timeLine = timeLine.Where(t => t.InReplyToScreenName == null).ToList();
            foreach (var t in timeLine)
            {
                timeLineListe.Add((Tweetinvi.Logic.Tweet) t);
            }
            return timeLineListe;
        }
        
        
        
        private string _searchInput;
    	public string SearchInput
     	{
            get { return _searchInput; }
            set { Set(ref _searchInput, value); }
        }
    	
    	private string _stringPostTweet;
    	public string StringPostTweet
        {
            get { return _stringPostTweet; }
            set { Set(ref _stringPostTweet, value); }
        }
    	
    	private RelayCommand _ISearch;
        public RelayCommand ISearch
        {
            get
            {
                if (_ISearch == null)
                    _ISearch = new RelayCommand(SearchInTwitter);
                return _ISearch;
            }
        }
        
        public void SearchInTwitter()
        {
        	if (!string.IsNullOrEmpty(this._searchInput))
            {
        		
                var tweets = Search.SearchTweets(this._searchInput);
                var timeLineCollection = new ObservableCollection<Tweetinvi.Logic.Tweet>();
                foreach (var tweet in tweets)
                {
                    timeLineCollection.Add((Tweetinvi.Logic.Tweet)tweet);
                }
                this.TimeLineTweets = timeLineCollection;
                this._searchInput = "";
            }
        	else if (!string.IsNullOrEmpty(this._searchInput))
            {
                var AnOtherUser = (Tweetinvi.Logic.User) User.GetUserFromScreenName(this._searchInput);
                if (AnOtherUser != null)
                {
                    this.Auser = AnOtherUser;
                    this.TimeLineTweets = this.getTimeLine(this.Auser);
                    this._searchInput = "";              
                }
            }
        }
 
        private RelayCommand _envoyerTweet;
        public RelayCommand sendTweet
        {
        	get
            {
                if (_envoyerTweet == null)
                    _envoyerTweet = new RelayCommand(EnvoyerTweet);
                return _envoyerTweet;
            }
        }
        

  
    	public void EnvoyerTweet()
    	{
    		var Tweetmsg = new ContentDialog();
    		
    		var stackPanel = new StackPanel();
    		
    		var textBox = new TextBox();
            textBox.MaxLength = 140;
            stackPanel.Children.Add(textBox);

            var textBlock = new TextBlock
            {
                Text = this.StringPostTweet
            };
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("YourTweet")
            };
            
            var buttonAddImage = new Button
            {
                Command = this.AddImage_click,
                Content = "Ajouter un fichier",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            stackPanel.Children.Add(buttonAddImage);
            Tweetmsg.Content = stackPanel;
            Tweetmsg.PrimaryButtonText = "DO IT!";
            Tweetmsg.SecondaryButtonText = "Annuler";
            
            var medias = this.MediasTweet;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
            	var tweet = Tweetinvi.Tweet.PublishTweet(textBox.Text, new PublishTweetOptionalParameters{Medias = medias});
            	this.MediasTweet = new List<IMedia>();
            }
            else
            {
            	this.EnvoyerTweet();
            }           
    	}
    	
    	public async void AddImage_click()
    	{
    		FileOpenPicker File = new FileOpenPicker();
    		File.ViewMode = PickerViewMode.Thumbnail;
   			File.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
		    
    		var file = await File.PickSingleFileAsync();
            await Task.Factory.StartNew(async () => {
                var stream = await file.OpenStreamForReadAsync();
                var file1 = new byte[(int)stream.Length];
                stream.Read(file1, 0, (int)stream.Length);
                this.MediasTweet.Add(Upload.UploadImage(file1));});
    		
    	}
    	
    	private RelayCommand<string> _Reply;
        public RelayCommand<string> Reply
        {
        	get
            {
                if (_Reply == null)
                    _Reply = new RelayCommand<string>(ReplyTweet);
                return _Reply;
            }
        }
        
        public void ReplyTweet(string idTweet)
        {
        	var selectedTweet = Tweetinvi.Tweet.GetTweet(long.Parse(idTweet));
        	
        	var Tweetmsg = new ContentDialog();
    		
    		var stackPanel = new StackPanel();
    		
    		var textBox = new TextBox();
            textBox.MaxLength = 140;
            stackPanel.Children.Add(textBox);

            var textBlock = new TextBlock
            {
                Text = this.StringPostTweet
            };
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("ReplyTweet")
            };
            
            var buttonAddImage = new Button
            {
                Command = this.AddImage_click,
                Content = "Ajouter un fichier",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            stackPanel.Children.Add(buttonAddImage);
            Tweetmsg.Content = stackPanel;
            Tweetmsg.PrimaryButtonText = "DO IT!";
            Tweetmsg.SecondaryButtonText = "Annuler";
            
            var medias = this.MediasTweet;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
            	var tweet = Tweetinvi.Tweet.PublishTweet(textBox.Text, new PublishTweetOptionalParameters{Medias = medias});
            	this.MediasTweet = new List<IMedia>();
            }
            else
            {
            	this.EnvoyerTweet();
            }   
        	
        	
        }
        
    	private RelayCommand<string> _retweet;
        public RelayCommand<string> Retweet
        {
        	get
            {
                if (_retweet == null)
                    _retweet = new RelayCommand<string>(ReTweet);
                return _retweet;
            }
        }
        
        public void ReTweet(string idTweet)
        {
        	var selectedTweetId = long.Parse(idTweet);
        	var tweet = Tweetinvi.Tweet.GetTweet(selectedTweetId);
        	
            if (tweet.Retweeted)
            {
                Tweetinvi.Tweet.UnFavoriteTweet(selectedTweetId);
            }
            else
            {
                Tweetinvi.Tweet.FavoriteTweet(selectedTweetId);
            }
            
            tweet =Tweetinvi.Tweet.GetTweet(selectedTweetId);
            var index = this.TimeLineTweets.IndexOf((this.TimeLineTweets.First(t => t.Id == long.Parse(idTweet))));
            this.TimeLineTweets[index] = (Tweet) tweet;
        }
        

        
        private RelayCommand<string> _Like;
        public RelayCommand<string> Like
        {
        	get
            {
                if (_Like == null)
                    _Like = new RelayCommand<string>(LikeTweet);
                return _Like;
            }
        }
        
        public void LikeTweet(string idTweet)
        {
        	var selectedTweetId = long.Parse(idTweet);
        	var tweet = Tweetinvi.Tweet.GetTweet(selectedTweetId);
        	
            if (tweet.Favorited)
            {
                Tweetinvi.Tweet.UnFavoriteTweet(selectedTweetId);
            }
            else
            {
                Tweetinvi.Tweet.FavoriteTweet(selectedTweetId);
            }
            
            tweet =Tweetinvi.Tweet.GetTweet(selectedTweetId);
            var index = this.TimeLineTweets.IndexOf((this.TimeLineTweets.First(t => t.Id == long.Parse(idTweet))));
            this.TimeLineTweets[index] = (Tweet) tweet;
        }
        
        private RelayCommand<string> _Delete;
        public RelayCommand<string> Delete
        {
        	get
            {
                if (_Delete == null)
                    _Delete = new RelayCommand<string>(DeleteTweet);
                return _Delete;
            }
        }
        
        public void DeleteTweet(string idTweet)
        {
        	var id = long.Parse(idTweet);
            var tweet = this._timeLineTweets.First(t => t.Id == id);
            if (tweet.CreatedBy.ScreenName == this._Auser.ScreenName)
            {
                this._timeLineTweets.Remove((Tweet)tweet);
                Tweetinvi.Tweet.DestroyTweet(id);
            }
        }
        
        
        /*private RelayCommand _deco;
        public RelayCommand Deco
        {
        	get
            {
                if (_deco == null)
                    _deco = new RelayCommand(deco_click);
                return _deco;
            }
        }
    	
    	public void deco_click()
		{
    		PinAuthorizer auth = null;
    		if (SuspensionManager.SessionState.ContainsKey("Authorizer"))
    		{
        		auth = SuspensionManager.SessionState["Authorizer"] as PinAuthorizer; 
    		}
 
    		if (auth == null || !auth.IsAuthorized)
    		{
        		Frame.Navigate(typeof(Views.MainPage));
        		return;
    		}

		} */
    }
}