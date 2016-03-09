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
			Auth.SetUserCredentials("HDtWgQ90KCQUgfF8yhpQLxj5U", "J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up", " 821562258-vZAwbc55MA8CHwRvRD1026GqeHhtAK2fwwPzYNFh", "WdJk9pVfLIzWyvUpac5k3AHyt4AY01x9EL4MRVn6lPqMk");
			Console.ReadLine();
			var user = User.User_GetCurrentUser();
			Console.WriteLine(use.Name);
			Timeline_GetUserTimeline(user);
			

			
		}
		
	}
}
