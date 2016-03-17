using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Tweetinvi;
using AppNet.ViewModels;
using AppNet.Model;
using Windows.UI.Xaml.Navigation;

namespace AppNet.Views
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();
        }
        
         protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var url =
                CredentialsCreator.GetAuthorizationURL(Connexion.getInstance().GetAppCredentials());

            TwitterPin.Source = new Uri(url);
        }
        
        
    }
}
