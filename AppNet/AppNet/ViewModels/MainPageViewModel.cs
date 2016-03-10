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
    	
    	var loggedUser = User.GetLoggedUser(userCredentials);
    	var homeTimeline = loggedUser.GetHomeTimeline();

    	private RelayCommand _Connexion;

        public RelayCommand Connexion
        {
            get
            {
                if (_Connexion == null)
                {
                    _Connexion = new RelayCommand(SetInputToOutput);
                }
                return _sentText;
            }

        }



    }
}

