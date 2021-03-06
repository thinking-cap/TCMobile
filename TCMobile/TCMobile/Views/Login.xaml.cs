﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Dynamic;
using TCMobile.Models;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        ICredentialsService storeService;
        
        public Login ()
		{

            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
            if (doCredentialsExist)
            {
                App.CredentialsService.DeleteCredentials();
                App.IsUserLoggedIn = false;

            }
        }
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
           
            string username;
            string password;
            username = Email.Text;
            password = Password.Text;
            Progress.IsVisible = true;
            Progress.IsRunning = true;
            Error.Text = "";
            TCMobile.LogInObj login = await userLogin.check(username, password);
            Styles s = new Styles();
            if (login.login == true)
            {
                bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
                if (!doCredentialsExist)
                {
                    App.CredentialsService.SaveCredentials(username, password,login.userId, login.firstName,login.lastName,login.homedomain, login.blobLoc);
                }

                Constants.Logo = new Uri(Constants.Url + "/FormatResource.ashx/programLearnerView_" + login.homedomain + "/logo.gif");

                App.IsUserLoggedIn = true;
                Constants.HeaderColour = login.headerColour;
                Constants.MenuBackgroundColour = login.menuBackgroundColour;
                Constants.MenuTextColour = login.menuTextColour;
               
                Application.Current.MainPage = new MainPage();
                Application.Current.Properties["HeaderColour"] = login.headerColour;
                Application.Current.Properties["MenuBGColour"] = login.menuBackgroundColour;
                Application.Current.Properties["MenuTextColour"] = login.menuTextColour;
                Application.Current.Properties["HeadingTextColour"] = login.headingTextColour;
               // Application.Current.Properties["Logo"] = Constants.Logo;
                Application.Current.Properties["ProgramID"] = login.homedomain;
                await Application.Current.SavePropertiesAsync();
                s.LabelColour("fontColor", Application.Current.Properties["HeadingTextColour"].ToString());

                Constants.StudentID = login.userId;
                Constants.firstName = login.firstName;
                Constants.lastName = login.lastName;
                Constants.BlobLocation = login.blobLoc;
                Progress.IsVisible = true;
                Progress.IsRunning = true;
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                string errorTxt = "";
                switch (login.status)
                {
                    case "invalid_password": errorTxt = "Password is incorrect, please check and try again.";break;
                    case "invalid_username": errorTxt = "Incorrect user name. Please check and try again.";break;
                    default: errorTxt = "There has been an issue with logging you in. Please try again.";break;
                }
                await Application.Current.MainPage.DisplayAlert("Warning", errorTxt, "OK");

                //Error.IsVisible = true;
                //Error.Text = login.status;
                Progress.IsVisible = false;
                Progress.IsRunning = false;
            }
           
        }

        async void OnForgotPasswordClicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new ForgotPassword());
    }

   
};