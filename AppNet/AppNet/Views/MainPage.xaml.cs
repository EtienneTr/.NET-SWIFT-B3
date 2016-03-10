using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Tweetinvi;

namespace AppNet.Views
{
    public sealed partial class MainPage : Page
    {
    	
    	const string ConsumerKey = "HDtWgQ90KCQUgfF8yhpQLxj5U";
        const string ConsumerSecret = "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up";
        const string AccessToken = "821562258-vZAwbc55MA8CHwRvRD1026GqeHhtAK2fwwPzYNFh";
        const string AccessTokenSecret = "WdJk9pVfLIzWyvUpac5k3AHyt4AY01x9EL4MRVn6lPqMk";

        public MainPage()
        {
            InitializeComponent();
            InitTwitterCredentials();
        }
        private static void InitTwitterCredentials()
        {
            var creds = new TwitterCredentials(AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret);
            Auth.SetUserCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);
            Auth.ApplicationCredentials = creds;
        }

        private void ButtonSendTweet_Click(object sender, RoutedEventArgs e)
        {
            PublishTweet();
        }

        public static async void PublishTweet(string text, string imageUrl)
        {
            var response = await WebRequest.Create(imageUrl).GetResponseAsync();
            var allBytes = new List<byte>();
            using (var imageStream = response.GetResponseStream())
            {
                // disable once SuggestUseVarKeywordEvident
                byte[] buffer = new byte[4000];
                int bytesRead;
                while ((bytesRead = await imageStream.ReadAsync(buffer, 0, 4000)) > 0)
                {
                    allBytes.AddRange(buffer.Take(bytesRead));
                }
            }
            var media = Upload.UploadBinary(allBytes.ToArray());
            Tweet.PublishTweet(text, new PublishTweetOptionalParameters
            {
                Medias = new List<IMedia> { media }
            });
        }
        
        

    }
}
