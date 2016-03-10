/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 03/09/2016
 * Time: 15:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Tweetinvi;

namespace AppNet
{
	/// <summary>
	/// Description of Program.
	/// </summary>
	public class Program
	{
		static void Main()
		{
			//Auth.SetUserCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up", " 821562258-vZAwbc55MA8CHwRvRD1026GqeHhtAK2fwwPzYNFh", "WdJk9pVfLIzWyvUpac5k3AHyt4AY01x9EL4MRVn6lPqMk");
	

			var appCredentials = new TwitterCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up");

			// Go to the URL so that Twitter authenticates the user and gives him a PIN code
			var url = CredentialsCreator.GetAuthorizationURL(appCredentials);
			
			// This line is an example, on how to make the user go on the URL
			Process.Start(url);
			
			// Ask the user to enter the pin code given by Twitter
			var pinCode = Console.ReadLine();
			
			// With this pin code it is now possible to get the credentials back from Twitter
			var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(pinCode, appCredentials);
			
			// Use the user credentials in your application
			Auth.SetCredentials(userCredentials);
			
			var user = User.User_GetCurrentUser();
			Console.WriteLine(use.Name);
			Timeline_GetUserTimeline(user);
		}
		
	}
}
