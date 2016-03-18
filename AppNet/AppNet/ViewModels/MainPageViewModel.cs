using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using System.IdentityModel;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using AppNet.Model;
using GalaSoft.MvvmLight.Command;



namespace AppNet.ViewModels
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
        
        public async void TweetLogin()
        {
        	
            if (!string.IsNullOrEmpty(_codeinput))
            {
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(Codeinput, Connexion.GetAppCredentials());
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

