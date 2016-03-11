using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace AppNet
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {

        public static Frame rootFrame { get; set; } 
 
        public App() 
        { 
            this.InitializeComponent(); 
            this.Suspending += this.OnSuspending; 
        } 
        protected override void OnLaunched(LaunchActivatedEventArgs e) 
        { 
#if DEBUG 
            if (System.Diagnostics.Debugger.IsAttached) 
            { 
                this.DebugSettings.EnableFrameRateCounter = true; 
            } 
#endif 
 
            rootFrame = Window.Current.Content as Frame; 
 
            // Do not repeat app initialization when the Window already has content, 
            // just ensure that the window is active 
            if (rootFrame == null) 
            { 
                // Create a Frame to act as the navigation context and navigate to the first page 
                rootFrame = new Frame(); 
 
                // TODO: change this value to a cache size that is appropriate for your application 
                //rootFrame.CacheSize = 1; 
 
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) 
                { 
                    // TODO: Load state from previously suspended application 
                } 
 
                // Place the frame in the current Window 
                Window.Current.Content = rootFrame; 
            } 
 
            if (rootFrame.Content == null) 
            { 
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments)) 
                { 
                    throw new Exception("Failed to create initial page"); 
                } 
            } 
            Window.Current.Activate(); 
        } 
 
 
        private async void OnSuspending(object sender, SuspendingEventArgs e) 
        { 
            var deferral = e.SuspendingOperation.GetDeferral(); 
 
            await SuspensionManager.SaveAsync(); 
             
            deferral.Complete(); 
        } 
    } 
}

