using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DOTNet
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Application
    {
        public App() {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
            Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
            Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
        }

        /// <summary>
        /// Invoqu� lorsque l'application est lanc�e normalement par l'utilisateur final.  D'autres points d'entr�e
        /// seront utilis�s par exemple au moment du lancement de l'application pour l'ouverture d'un fichier sp�cifique.
        /// </summary>
        /// <param name="e">D�tails concernant la requ�te et le processus de lancement.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            // Ne r�p�tez pas l'initialisation de l'application lorsque la fen�tre comporte d�j� du contenu,
            // assurez-vous juste que la fen�tre est active
            if (rootFrame == null)
            {
                // Cr�ez un Frame utilisable comme contexte de navigation et naviguez jusqu'� la premi�re page
                rootFrame = new Frame();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'�tat de l'application pr�c�demment suspendue
                }

                // Placez le frame dans la fen�tre active
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Quand la pile de navigation n'est pas restaur�e, acc�dez � la premi�re page,
                // puis configurez la nouvelle page en transmettant les informations requises en tant que
                // param�tre
                rootFrame.Navigate(typeof(Views.MainPage), e.Arguments);
            }
            // V�rifiez que la fen�tre actuelle est active
            Window.Current.Activate();
        }

        /*public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }*/
    }
}

