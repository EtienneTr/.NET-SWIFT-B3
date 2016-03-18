/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 03/16/2016
 * Time: 10:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;
using AppNet.Model;
using Tweetinvi.Core.Credentials;

namespace AppNet.Views
{
	/// <summary>
	/// Interaction logic for CodeUserView.xaml
	/// </summary>
	public sealed partial class CodeUserView : Page
	{
		public CodeUserView()
		{
            InitializeComponent();
		}
		
		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        	var appCredential = new TwitterCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up");
 
        	var url = CredentialsCreator.GetAuthorizationURL(appCredential);

            this.TwitterPin.Source = url;
        	
        }
		
	}
}