using System.Collections.Generic;
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

namespace DOTNet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
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
            this.NavigationService.Navigate(typeof(Views.CodeUserView));
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
                var appCredential = new TwitterCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up");
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(Codeinput, appCredential);
                Auth.SetCredentials(userCredentials);

                if (userCredentials != null)
                {

                    var account = new Token(userCredentials.AccessToken, userCredentials.AccessTokenSecret);
                    AccessToken.SaveAccountData(account);
                    this.NavigationService.Navigate(typeof(Views.LoggedUserView));
                }

            }
        }
    }
}

