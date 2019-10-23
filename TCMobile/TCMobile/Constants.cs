using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace TCMobile
{
    class Constants
    {
        public static  string LocalFolder;
        // public static string Url = "https://192.168.0.126";
        //public static string Url = "https://stable.thinkingcap.com";// for local testing
        public static string Url = "https://camp.thinkingcap.com";// for beta testing
        public static string Key = "9b5ibe91jlq7o9nr4saglg";
        public static string ForgotPassword = Url + "/Login/EmailPassword.aspx?modal=true";
        public static string StudentCatalogue = Url +"/Mobile/Catalogue.ashx";
        public static string LearningPaths = Url + "/Mobile/GetLPS.ashx";
        public static string LearningPath = Url + "/Mobile/LPDetails.ashx";
        public static string SetCMI = Url + "/WebService/LearnerManagement.asmx/SetSCORMValues";
        public static string GetCMI = Url + "/WebService/LearnerManagement.asmx/GetSCORMDetails";
        public static string EnrollUser = Url + "/WebService/learnermanagement.asmx?op=EnrollStudent";
        public static ImageSource Logo { get; set; }
        public static string StudentID = "740ad541-661a-43ea-b825-cec3788f2c3a";
        //public static string ProgramID = "612e7450-2370-45b7-b2f9-8686b1d7c7f9";
        public static string ProgramID = "00000000-0000-0000-0000-000000000000";
        public static string LoginURL = Url + "/Mobile/Login.ashx";
        public static string firstName;
        public static string lastName;
        public static string BlobLocation;
        public static bool isOnline;
        public static double deviceWidth;
        public static string WifiOnly { get; set; }
        public static string HeaderColour { get; set; }
        public string FreeSpace { get; set; }
        public static string MenuBackgroundColour { get; set; }
        public static string MenuTextColour { get; set; }





    }

    
}
