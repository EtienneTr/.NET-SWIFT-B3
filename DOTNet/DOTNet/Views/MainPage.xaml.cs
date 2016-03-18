using System;
using Windows.UI.Xaml.Controls;
using DOTNet.Model;
using DOTNet.ViewModels;
using Tweetinvi;

namespace DOTNet.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
    }
}
