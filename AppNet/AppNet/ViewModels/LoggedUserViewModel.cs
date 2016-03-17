/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 15/03/2016
 * Time: 14:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using System.Linq;
using Windows.Storage; 
using Windows.Storage.Pickers; 
using Windows.UI.Xaml; 
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Navigation; 
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using Tweet = Tweetinvi.Logic.Tweet;
using User = Tweetinvi.User;

namespace AppNet.ViewModels
{
    public class LoggedUserViewModel : ViewModelBase
    {
    	private List<Tweetinvi.Core.Interfaces.DTO.IMedia> MediasTweet;
    	
    	public LoggedUserViewModel()
        {
            this.user = (Tweetinvi.Logic.User) User.GetAuthenticatedUser();
            this.TimeLineTweets = getTimeLineO(this.user);
            

            this.MediasTweet = new List<Tweetinvi.Core.Interfaces.DTO.IMedia>();
        }
    
    
    	
    	
    	private Tweetinvi.Logic.User _user;
    	public Tweetinvi.Logic.User user
		{
			get{return _user; }
            set{ Set(ref _user, value);}
		}
    	
    	private ObservableCollection<Tweet> _timeLineTweets;

        public ObservableCollection<Tweet> TimeLineTweets
        {
            get { return _timeLineTweets; }
            set
            {
                Set(ref _timeLineTweets, value);
            }
        }
        
        public ObservableCollection<Tweetinvi.Logic.Tweet> getTimeLine(Tweetinvi.Logic.User user)
        {
            var timeLine = Timeline.GetUserTimeline(user);
            var timeLineListe = new ObservableCollection<Tweet>();
            timeLine = timeLine.Where(t => t.InReplyToScreenName == null).ToList();
            foreach (var t in timeLine)
            {
                timeLineCollection.Add((Tweetinvi.Logic.Tweet) t);
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
    	
    	private RelayCommand _Search;
        public RelayCommand Search
        {
            get
            {
                if (_Search == null)
                    _Search = new RelayCommand(SearchInTwitter);
                return _Search;
            }
        }
        
        public void SearchInTwitter()
        {
        	if (!string.IsNullOrEmpty(this._searchInput))
            {
                var tweets = Search.SearchTweets(this._searchInput);
                var timeLineCollection = new ObservableCollection<Tweet>();
                foreach (var tweet in tweets)
                {
                    timeLineCollection.Add((Tweetinvi.Logic.Tweet)tweet);
                }
                this.TimeLineTweets = timeLineCollection;
                this._searchInput = "";
            }
        	else if (!String.IsNullOrEmpty(this._searchInput))
            {
                var AnOtherUser = (Tweetinvi.Logic.User) User.GetUserFromScreenName(this._searchInput);
                if (AnOtherUser != null)
                {
                    this.Selecteduser = AnOtherUser;
                    this.TimeLineTweets = this.getTimeLineObservableCollection(this.Selecteduser);
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
    		var msg = new ContentDialog();
    		
    		var stackPanel = new StackPanel();
    		
    		var textBox = new TextBox();
            textBox.KeyUp += new KeyEventHandler(checkTweetCharacter);
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
            dialog.Content = stackPanel;
            dialog.PrimaryButtonText = "DO IT!";
            dialog.SecondaryButtonText = "Annuler";
            
            var medias = this.MediasTweet;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
            	var tweet = Tweetinvi.Tweet.PublishTweet(textBox.Text, new PublishTweetOptionalParameters{Medias = medias});
            	this._nbCharacterTweet = 140;
            	this.MediasTweet = new List<IMedia>();
            }
            else
            {
            	this.EnvoyerTweet();
            }           
    	}
    	
    	public async void AddImage_click()
    	{
    		// disable once SuggestUseVarKeywordEvident
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
                    _Reply = new RelayCommand(ReplyTweet);
                return _Reply;
            }
        }
        
        public void ReplyTweet(string idTweet)
        {
        	var selectedTweet = Tweetinvi.Tweet.GetTweet(long.Parse(tweetId));
        }
    	private RelayCommand<string> _retweet;
        public RelayCommand<string> Retweet
        {
        	get
            {
                if (_retweet == null)
                    _retweet = new RelayCommand(ReTweet);
                return _retweet;
            }
        }
        
        public void ReTweet(string idTweet)
        {}
        

        
        private RelayCommand<string> _Like;
        public RelayCommand<string> Like
        {
        	get
            {
                if (_Like == null)
                    _Like = new RelayCommand(LikeTweet);
                return _Like;
            }
        }
        
        public void LikeTweet(string idTweet)
        {}
        
        private RelayCommand<string> _Delete;
        public RelayCommand<string> Delete
        {
        	get
            {
                if (_Delete == null)
                    _Delete = new RelayCommand(DeleteTweet);
                return _Delete;
            }
        }
        
        public void DeleteTweet(string idTweet)
        {
        	var id = long.Parse(idTweet);
            var tweet = this._timeLineTweets.First(t => t.Id == id);
            if (tweet.CreatedBy.ScreenName == this._user.ScreenName)
            {
                this._timeLineTweets.Remove((Tweet)tweet);
                Tweetinvi.Tweet.DestroyTweet(id);
            }
        }
        
        private RelayCommand _deco;
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

		}
    	
    }
}