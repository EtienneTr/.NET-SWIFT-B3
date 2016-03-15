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
    	private string _input;
    	
    	private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e) 
        { 
            // TODO: Create an appropriate data model for your problem domain to replace the sample data 
            DefaultViewModel["AuthRequired"] = true; 
            DefaultViewModel["ServiceCall"] = false; 
 
            var oAuth = new OAuthInfo(); 
            oAuth.ConsumerKey = TwitterConsumerKey; 
            oAuth.ConsumerSecret = TwitterConsumerSecret; 
 
            m_TwitterClient = new TwitterClient(oAuth); 
        }
        
    	public void TweetLogin(object sender, RoutedEventArgs e)
		{
    		// Create a new set of credentials for the application
			var appCredentials = new TwitterCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up");
			
			// Go to the URL so that Twitter authenticates the user and gives him a PIN code
			var url = CredentialsCreator.GetAuthorizationURL(appCredentials);
			
			// This line is an example, on how to make the user go on the URL
			Process.Start(url);
			
			// Ask the user to enter the pin code given by Twitter
			var pinCode = Console.ReadLine();
			
			// With this pin code it is now possible to get the credentials back from Twitter
			var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(pinCode, appCredentials);
			
			// Use the user credentials in your application
			Auth.SetCredentials(userCredentials);
    		PinAuthorizer auth = null;
    		if (SuspensionManager.SessionState.ContainsKey("Authorizer"))
    		{
        		auth = SuspensionManager.SessionState["Authorizer"] as PinAuthorizer; 
    		}
 
    		if (auth == null || !auth.IsAuthorized)
    		{
        		Frame.Navigate(typeof(Views.LoggedUserView));
        		return;
    		}

		}
    }
}

