using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;


namespace TCMobile
{
    class Constants
    {
        public static  string LocalFolder;
        // public static string Url = "https://192.168.0.126";
        public static string Url = "https://stable.thinkingcap.com";
        public static string StudentCatalogue = Url +"/Mobile/Catalogue.ashx";
        public static string LearningPaths = Url + "/Mobile/GetLPS.ashx";
        public static string SetCMI = Url + "/WebService/LearnerManagement.asmx/SetSCORMValues";
        public static string GetCMI = Url + "/WebService/LearnerManagement.asmx/GetSCORMDetails";
        public static string StudentID = "740ad541-661a-43ea-b825-cec3788f2c3a";
        //public static string ProgramID = "612e7450-2370-45b7-b2f9-8686b1d7c7f9";
        public static string ProgramID = "00000000-0000-0000-0000-000000000000";
        public static string LoginURL = Url + "/Mobile/Login.ashx";
        public static string firstName;
        public static string lastName;
        public static string BlobLocation;
        public static bool isOnline;

       



    }

    
}
