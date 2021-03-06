﻿using TCMobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            NavigationPage.SetHasNavigationBar(this, false);
            MenuPages.Add((int)MenuItemType.Catalogue, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
              
                switch (id)
                {
                    case (int)MenuItemType.Catalogue:
                        MenuPages.Add(id, new NavigationPage(new Catalogue()) { BarBackgroundColor = Color.FromHex(Constants.HeaderColour) });
                        break;
                    case (int)MenuItemType.MyCourses:
                        MenuPages.Add(id, new NavigationPage(new MyCourses()) { BarBackgroundColor = Color.FromHex(Constants.HeaderColour) });
                        break;
                    case (int)MenuItemType.LearningPaths:
                        MenuPages.Add(id, new NavigationPage(new LearningPaths()) { BarBackgroundColor = Color.FromHex(Constants.HeaderColour) });
                        break;
                    case (int)MenuItemType.MyTranscripts:
                        MenuPages.Add(id, new NavigationPage(new MyTranscripts()) { BarBackgroundColor = Color.FromHex(Constants.HeaderColour) });
                        break;
                    case (int)MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new Settings()) { BarBackgroundColor = Color.FromHex(Constants.HeaderColour) });
                        break;
                    case (int)MenuItemType.Logout:
                        MenuPages.Add(id, new NavigationPage(new Login()));
                        break;
                }
            }


            if (id == (int)MenuItemType.Logout)
            {
                Application.Current.MainPage = new Login();
                await Navigation.PushAsync(new Login());
            }
            else
            {
                var newPage = MenuPages[id];

                if (newPage != null && Detail != newPage)
                {
                    Detail = newPage;
                    
                    if (Device.RuntimePlatform == Device.Android)
                        await Task.Delay(100);

                    IsPresented = false;
                }
            }
        }

        public void Logout(int id)
        {
            bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
            if (doCredentialsExist)
                App.CredentialsService.DeleteCredentials();           
        }
    }
}