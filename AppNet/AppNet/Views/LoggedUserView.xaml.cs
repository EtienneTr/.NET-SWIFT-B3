﻿/*
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
	public partial class LoggedUserView : Page
	{
		public LoggedUserView()
		{
			InitializeComponent();
            this.Loaded += OAuthPage_Loaded;
    		OAuthWebBrowser.LoadCompleted += OAuthWebBrowser_LoadCompleted;
        }
		
		
	}
}