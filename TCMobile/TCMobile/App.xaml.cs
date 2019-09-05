using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TCMobile.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using TCMobile.Data;
using System.IO;
using Xamarin.Essentials;
using TCMobile.CustomControls;
using TCMobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TCMobile
{
    public partial class App : Application
    {

        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
       
        public static string CurrentLP { get; set; }
        public static bool IsUserLoggedIn { get; set; }
        static LMSDataBase database;
        public static Catalogue CourseCatalogue { get; set; }

        public static String LocalFolder { get; set; }

        public static string AppName { get { return "TCLMS"; } }

        public static dynamic Currentdownload { get; set; }

        public static ICredentialsService CredentialsService { get; private set; }
        public App()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                Constants.isOnline = true;
            }
            else
            {
                Constants.isOnline = false;
            }

            
            CredentialsService = new CredentialsService();
            InitializeComponent();
            Constants.LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            Constants.WifiOnly = App.Current.Properties.ContainsKey("WiFi") ? App.Current.Properties["WiFi"].ToString() : "False";

            // load the lms settings
            /// GetLMSSettings();
            Styles s = new Styles();
            try
            {
                if (Application.Current.Properties.ContainsKey("ProgramID"))
                    Constants.Logo = new Uri(Constants.Url + "/FormatResource.ashx/programLearnerView_" + Application.Current.Properties["ProgramID"].ToString() + "/logo.gif");
                if (Application.Current.Properties.ContainsKey("HeaderColour"))
                    Constants.HeaderColour = Application.Current.Properties["HeaderColour"].ToString();
                // Constants.Logo = new Uri(Constants.Url + "/FormatResource.ashx/programLearnerView_" + CredentialsService.HomeDomain + "/logo.gif");
                if (Application.Current.Properties.ContainsKey("MenuBGColour"))
                    Constants.MenuBackgroundColour = Application.Current.Properties["MenuBGColour"].ToString();

                if (Application.Current.Properties.ContainsKey("MenuTextColour"))
                    Constants.MenuTextColour = Application.Current.Properties["MenuTextColour"].ToString();
            }catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
            if (CredentialsService.DoCredentialsExist() && !String.IsNullOrEmpty(CredentialsService.HomeDomain) && !String.IsNullOrEmpty(CredentialsService.UserID))
            {
                // added a try catch just incase the param hasn't been created. 
                // then we need to force a log in to retrieve the blob location
                try
                {
                   
                   
                    s.LabelColour("fontColor", Application.Current.Properties["HeadingTextColour"].ToString());
                    s.SpinnerColour("bgColor", Application.Current.Properties["HeadingTextColour"].ToString());
                    // need to set a variable for the width of the current device 
                    if (Application.Current.Properties.ContainsKey("HeaderColour"))
                        Constants.HeaderColour = Application.Current.Properties["HeaderColour"].ToString();
                    Constants.Logo = new Uri(Constants.Url + "/FormatResource.ashx/programLearnerView_" + CredentialsService.HomeDomain + "/logo.gif");
                    if (Application.Current.Properties.ContainsKey("MenuBGColour"))
                        Constants.MenuBackgroundColour = Application.Current.Properties["MenuBGColour"].ToString();

                    if (Application.Current.Properties.ContainsKey("MenuTextColour"))
                        Constants.MenuTextColour = Application.Current.Properties["MenuTextColour"].ToString();
                    Constants.BlobLocation = CredentialsService.BlobLoc;
                     MainPage = new MainPage();
                    Constants.deviceWidth = Application.Current.MainPage.Width;



                }
                catch (Exception e)
                {
                    MainPage = new NavigationPage(new Login());
                    Constants.deviceWidth = Application.Current.MainPage.Width;
                }

            }
            else {
                MainPage = new NavigationPage(new Login());
                Constants.deviceWidth = Application.Current.MainPage.Width;
            }
                
        }


        public void OnMenu(Object Sender, EventArgs args)
        {

        }
        public static LMSDataBase Database
        {
            get
            {
                if (database == null)
                {
                    database = new LMSDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TodoSQLite.db3"));
                }
                return database;
            }
        }
         
        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("ios=35d5cf8e-f79c-4e64-9390-d7169eda5555;" 
                + "android=6f839c85-8c8a-4e6a-89ac-d4b82b3d49b1;", typeof(Analytics), typeof(Crashes));

           
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
