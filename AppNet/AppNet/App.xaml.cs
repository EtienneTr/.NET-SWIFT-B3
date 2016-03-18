
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.Threading;
using Tweetinvi;
using AppNet.ViewModels;
using AppNet.Model;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using Windows.ApplicationModel.Activation;

namespace AppNet
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {

        public App() 
        {
            InitializeComponent();
    	}

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }
    } 
}

