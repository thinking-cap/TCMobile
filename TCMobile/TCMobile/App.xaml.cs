using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TCMobile
{
    public partial class App : Application
    {

        public static bool IsUserLoggedIn { get; set; }
        public App()
        {
            InitializeComponent();

            if (IsUserLoggedIn)
            {
                MainPage = new MainPage();
            }
            else {
                MainPage = new NavigationPage(new Login());
            }
                
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("ios=7aae5c12-87d4-4d6b-9d81-7823c9b6f72c;" + "uwp={bc982458-35a2-4f4c-bbb9-82d98349e3e8;};" + "android={6f839c85-8c8a-4e6a-89ac-d4b82b3d49b1;}", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
