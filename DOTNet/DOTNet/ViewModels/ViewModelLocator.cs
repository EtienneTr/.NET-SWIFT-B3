using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOTNet.ViewModels;
using GalaSoft.MvvmLight.Views;
using DOTNet.Views;

namespace DOTNet.ViewModels
{
    class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<LoggedUserViewModel>();

            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
        }

        public MainPageViewModel Login
        {
            get { return ServiceLocator.Current.GetInstance<MainPageViewModel>(); }
        }

        public LoggedUserViewModel LoggedUser
        {
            get { return ServiceLocator.Current.GetInstance<LoggedUserViewModel>(); }

        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("CodeUserView", typeof(CodeUserView));
            // navigationService.Configure("key1", typeof(OtherPage1));
            // navigationService.Configure("key2", typeof(OtherPage2))

            return navigationService;
        }
    }
}
