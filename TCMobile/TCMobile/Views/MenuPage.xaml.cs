using TCMobile.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            Icon = "hamburger.png";
            Title = "Menu";
            CredentialsService credentials = new CredentialsService();

            // custom renderer
            


            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Catalogue, Title="Catalogue" },
                new HomeMenuItem {Id = MenuItemType.MyCourses, Title="My Courses" },
                new HomeMenuItem {Id = MenuItemType.MyTranscripts, Title="My Transcripts"},
                new HomeMenuItem {Id = MenuItemType.Logout, Title="Logout"}
            };
            bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
            UserImage.Source = Constants.Url + "/ViewPhoto.aspx?UserID=" + credentials.UserID;
            string first = "Learner";
            string last = "";
            if (doCredentialsExist)
            {
                try
                {
                    first = App.CredentialsService.FirstName;
                    last = App.CredentialsService.LastName;
                }
                catch { }
            }

            Constants.firstName = first;
            Constants.lastName = last;

            UserName.Text = "Hello " + first + " " + last;
            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
       
    }
}