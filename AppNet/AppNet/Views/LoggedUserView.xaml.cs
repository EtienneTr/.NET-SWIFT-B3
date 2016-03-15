/*
 * Created by SharpDevelop.
 * User: Ihab
 * Date: 03/11/2016
 * Time: 09:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Tweetinvi;

namespace AppNet.Views
{
	/// <summary>
	/// Interaction logic for LoggedUserView.xaml
	/// </summary>
	public partial class LoggedUserView : ViewModelBase
	{
		public LoggedUserView()
		{
			InitializeComponent();
            this.Loaded += OAuthPage_Loaded;
    		OAuthWebBrowser.LoadCompleted += OAuthWebBrowser_LoadCompleted;
        }
		
		public static Tweetinvi.Logic.User _selectedUser
		{
			get{return SeletedUser; }
            // disable once ValueParameterNotUsed
            set{ref SeletedUser, value}
		}
		
    	void deco_click(object sender, RoutedEventArgs e)
		{
    		PinAuthorizer auth = null;
    		if (SuspensionManager.SessionState.ContainsKey("Authorizer"))
    		{
        		auth = SuspensionManager.SessionState["Authorizer"] as PinAuthorizer; 
    		}
 
    		if (auth == null || !auth.IsAuthorized)
    		{
        		Frame.Navigate(typeof(Views.LoggedUserView));
        		return;
    		}

		}
    	
    	public async void TweetButton_Click(object sender, RoutedEventArgs e)
    	{
    		var response = await WebRequest.Create(imageUrl).GetResponseAsync();
            var allBytes = new List<byte>();
            using (var imageStream = response.GetResponseStream())
            {
                byte[] buffer = new byte[4000];
                int bytesRead;
                while ((bytesRead = await imageStream.ReadAsync(buffer, 0, 4000)) > 0)
                {
                    allBytes.AddRange(buffer.Take(bytesRead));
                }
            }
            var media = Upload.UploadBinary(allBytes.ToArray());
            Tweet.PublishTweet(text, new PublishTweetOptionalParameters
            {
                Medias = new List<IMedia> { media }
            });    	
    	}
    	
    	public void AddImage_click(object sender, RoutedEventArgs e)
    	{
    		if (FileUpload1.HasFile)
    		{
    			string fileExt = System.IO.Path.GetExtension(FileUpload1.FileName);
    			if (fileExt == ".mp3" ==".jpg" ==".gif" ==".avi" ==".png")
    			{    				
    				try      
    				{
    					FileUpload1.SaveAs("C:\\Uploads\\" + FileUpload1.FileName);
			            Label1.Text = "File name: " + FileUpload1.PostedFile.FileName + "<br>" + FileUpload1.PostedFile.ContentLength + " kb<br>" +
			                        "Content type: " + FileUpload1.PostedFile.ContentType;
    				}
    				catch (Exception ex)       
    				{          
    					Label1.Text = "Erreur: " + ex.Message.ToString();
    				}        
    			}       
    			else      
    			{       
    				Label1.Text = "Fichier non autorisé";     
    			}   
    		}       
    		else      
    		{           
    			Label1.Text = "Aucun fichier selectionné.";     
    		}
    	}
	}
}