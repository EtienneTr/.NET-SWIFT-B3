/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 03/17/2016
 * Time: 11:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;

namespace AppNet.Model
{
	class Connexion
	{
		private readonly string consumerKey;
        private readonly string consumerSecret;
        private TwitterCredentials appCredential;

        public Connexion()
        {
        	this.consumerKey = "HDtWgQ90KCQUgfF8yhpQLxj5U";
        	this.consumerSecret ="J4yg5hVrlvCocWoT4lWCqQXvaZ7C5YAfw9wgGZwZF5YuFY46Up";
        	this.appCredential = new TwitterCredentials(this.consumerKey, this.consumerSecret);
        }
        
        public TwitterCredentials GetAppCredentials()
        {
            return this.appCredential;
        }

        public string getConsumerKey()
        {
            return this.consumerKey;
        }

        public string getConsumerSecret()
        {
            return this.consumerSecret;
        }
        
        
        
        public  static TwitterConnexion getInstance()
        {
            return instance;
        }
	}
}
