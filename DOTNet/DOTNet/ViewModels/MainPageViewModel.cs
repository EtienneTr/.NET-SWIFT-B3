using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using DOTNet.Model;
using CredentialsCreator = Tweetinvi.CredentialsCreator;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DOTNet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        TwitterCredentials TheappCredential;

        public MainPageViewModel()
        {
            TheappCredential = new TwitterCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up");
            TwitterUrl = new Uri(CredentialsCreator.GetAuthorizationURL(TheappCredential));
        }

        private string _codeinput;
        public string Codeinput
        {
            get { return _codeinput; }
            set { Set(ref _codeinput, value); }
        }

        private RelayCommand _GetCode;
        public RelayCommand GetCode
        {
            get
            {
                if (_GetCode == null)
                    _GetCode = new RelayCommand(GetPinConnection);
                return _GetCode;
            }
        }

        public void GetPinConnection()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Views.CodeUserView));
            // navigationService.NavigateTo("CodeUserView", "");
            // this.NavigationService.Navigate(new Uri("CodeUserView.xaml", UriKind.RelativeOrAbsolute));
            //this.NavigationService.Navigate(typeof(Views.CodeUserView));
        }

        private RelayCommand _connection;
        public RelayCommand Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new RelayCommand(TweetLogin);
                return _connection;
            }
        }

        public void TweetLogin()
        {

            if (!string.IsNullOrEmpty(_codeinput))
            {
               
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(Codeinput, TheappCredential);
                Auth.SetCredentials(userCredentials);

                if (Auth.Credentials != null)
                {

                    var account = new Token(userCredentials.AccessToken, userCredentials.AccessTokenSecret);
                    AccessToken.SaveAccountData(account);
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(Views.LoggedUserView));
                   // this.NavigationService.Navigate(typeof(Views.LoggedUserView));
                }

            }
        }

        private Uri _TwitterUrl;
        public Uri TwitterUrl
        {
            get { return _TwitterUrl; }
            set { Set(ref _TwitterUrl, value); }
        }
    }
}

