using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOTNet.ViewModels;

namespace DOTNet.ViewModels
{
    class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<LoggedUserViewModel>();
        }

        public MainPageViewModel Login
        {
            get { return ServiceLocator.Current.GetInstance<MainPageViewModel>(); }
        }

        public LoggedUserViewModel LoggedUser
        {
            get { return ServiceLocator.Current.GetInstance<LoggedUserViewModel>(); }

        }
    }
}
