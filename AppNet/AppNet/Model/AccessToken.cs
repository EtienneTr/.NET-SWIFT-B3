/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 03/17/2016
 * Time: 21:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppNet.Model
{
	/// <summary>
	/// Description of AccessToken.
	/// </summary>
	/// 
	class Token
	{
		public string token;
        public string tokenSecret;

        public Token(string token, string tokenSecret)
        {
            this.token = token;
            this.tokenSecret = tokenSecret;
        }
	}
	static class AccessToken
	{

        
		public static async void SaveAccountData(Token tokens)
        {
            var folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("config.json",CreationCollisionOption.ReplaceExisting);
            string tokensJson = JsonConvert.SerializeObject(tokens);
            await Windows.Storage.FileIO.WriteTextAsync(file,tokensJson);
        }

        public static Token ReadTokens()
        {
            var tokens = JsonConvert.DeserializeObject<Token>(File.ReadAllText(ApplicationData.Current.LocalFolder.Path + "\\config.json"));

            return tokens;
        }
	}
}
