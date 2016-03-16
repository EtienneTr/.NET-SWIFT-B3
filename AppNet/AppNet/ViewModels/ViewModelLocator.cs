/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 15/03/2016
 * Time: 14:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;


namespace AppNet.ViewModels
{
	/// <summary>
	/// Description of ViewModelLocator.
	/// </summary>
	public class ViewModelLocator
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
		
		public LoggedUserViewModel User
		{
			get { return ServiceLocator.Current.GetInstance<LoggedUserViewModel>(); }

		}
	}
}
