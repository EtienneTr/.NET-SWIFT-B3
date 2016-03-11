using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Tweetinvi;

namespace AppNet.Views
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
            this.Loaded += OAuthPage_Loaded;
    		OAuthWebBrowser.LoadCompleted += OAuthWebBrowser_LoadCompleted;
        }
        
    	void TweetButton_Click(object sender, RoutedEventArgs e)
		{
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
    	
    	void OAuthPage_Loaded(object sender, EventArgs e)
        {

            // disable once ConvertToConstant.Local
            var oauth_consumer_key = "HDtWgQ90KCQUgfF8yhpQLxj5U";
            // disable once ConvertToConstant.Local
            var oauth_consumer_secret = "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up";

            if (Request["oauth_token"] == null)
            {
                OAuthTokenResponse reqToken = OAuthUtility.GetRequestToken(oauth_consumer_key,oauth_consumer_secret,
                    Request.Url.AbsoluteUri);
                Response.Redirect(string.Format("http://twitter.com/oauth/authorize?oauth_token={0}",
                    reqToken.Token));
            }
            else
            {
                string requestToken = Request["oauth_token"].ToString();
                string pin = Request["oauth_verifier"].ToString();
                var tokens = OAuthUtility.GetAccessToken(
                    oauth_consumer_key,
                    oauth_consumer_secret,
                    requestToken,
                    pin);
                // disable once SuggestUseVarKeywordEvident
                OAuthTokens accesstoken = new OAuthTokens()
                {
                    AccessToken = tokens.Token,
                    AccessTokenSecret = tokens.TokenSecret,
                    ConsumerKey = oauth_consumer_key,
                    ConsumerSecret = oauth_consumer_secret
                };

            }

        }
    }
}
