/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 03/16/2016
 * Time: 10:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Windows.UI.Xaml.Controls;
using Tweetinvi;
using AppNet.Model;
using Windows.UI.Xaml.Navigation;

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
 
        	var url = CredentialsCreator.GetAuthorizationURL(Connexion.GetAppCredentials());

            this.TwitterPin.Source = url;
        	
        }
		
	}
}