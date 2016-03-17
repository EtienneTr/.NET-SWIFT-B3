using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using TwitterDotNet.Services.TweetinviAPI;
using TwitterDotNet.Services.AccountManager;


namespace AppNet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
    	public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            AppCredentials = new TwitterCredentials(TweetinviData.ConsumerKey, TweetinviData.ConsumerSecret);
            var url = CredentialsCreator.GetAuthorizationURL(AppCredentials);
            Uri targeturi = new Uri(url);

            NavigationService.Navigate(typeof(Views.LoginPage), targeturi);
            
            await Task.CompletedTask;
        }
    	
    	private Uri _webview;
        public Uri WebView { get { return _webview; } set { _webview = value; RaisePropertyChanged(); } }
        
    	private string _codeinput;
    	public string Codeinput
        {
            get { return _codeinput; }
            set { Set(ref _codeinput, value); }
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

            }
        }	
    }
}

