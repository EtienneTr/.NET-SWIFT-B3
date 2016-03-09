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
            Loaded += OnLoaded;
        }
        
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Auth.SetUserCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up", "821562258-vZAwbc55MA8CHwRvRD1026GqeHhtAK2fwwPzYNFh", "WdJk9pVfLIzWyvUpac5k3AHyt4AY01x9EL4MRVn6lPqMk");

            if (Auth.Credentials == null ||
                string.IsNullOrEmpty(Auth.Credentials.ConsumerKey) ||
                string.IsNullOrEmpty(Auth.Credentials.ConsumerSecret) || 
                string.IsNullOrEmpty(Auth.Credentials.AccessToken) ||
                string.IsNullOrEmpty(Auth.Credentials.AccessTokenSecret) ||
                Auth.Credentials.AccessToken == "ACCESS_TOKEN")
            {
                Message.Text = "Please enter your credentials in the MainPage.xaml.cs file";
            }
            else
            {
                var user = User.GetLoggedUser();
                Message.Text = string.Format("Bonjour ", user.Name);

                RunSampleStream();
            }
        }
        
        private void RunSampleStream()
        {
            var uiDispatcher = Dispatcher;
            var s = Stream.CreateSampleStream();

            s.TweetReceived += (o, args) =>
            uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
				Message.Text = args.Tweet.ToString();
			});

            s.StartStreamAsync();
        }

    }
}
