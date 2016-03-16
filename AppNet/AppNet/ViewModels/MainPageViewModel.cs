using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace AppNet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
    	//Methode avec le pin
    	private string _codeinput;
    	public string Codeinput
        {
            get { return _codeinput; }
            set { Set(ref _codeinput, value); }
        }
    	
    	private RelayCommand _getCode;
        public RelayCommand GetCode
        {
            get
            {
                if (_getCode == null)
                    _getCode = new RelayCommand(GetCodeUser);
                return _getCode;
            }
        }
        
        public void GetCodeUser()
        {
        	
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
        
        public async void TweetLogin()
        {
            if (!string.IsNullOrEmpty(_codeinput))
            {
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(Codeinput, TwitterConnectionInfoSingleton.getInstance().GetAppCredentials());

                if (userCredentials != null)
                {
                    Auth.SetCredentials(userCredentials);
                    var account = new Token(userCredentials.AccessToken, userCredentials.AccessTokenSecret);
                    AccountToken.SaveAccountData(account);
                    this.NavigationService.Navigate(typeof(Views.LoggedUserView));
                }
                else
                {
                    var msgDialogue = new MessageDialog("Erreur de connexion, Mauvais code!");
                    await msgDialogue.ShowAsync();
                }
            }

            if (File.Exists(ApplicationData.Current.LocalFolder.Path + "\\config.json"))
            {
                var tokens = AccountToken.ReadTokens();
                var userCredentials = Auth.CreateCredentials(TwitterConnectionInfoSingleton.getInstance().getConsumerKey(), TwitterConnectionInfoSingleton.getInstance().getConsumerSecret(), tokens.token, tokens.tokenSecret);
                Auth.SetCredentials(userCredentials);
                this.NavigationService.Navigate(typeof(Views.LoggedUserView));
            }
        }	
    }
}

