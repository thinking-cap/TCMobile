using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TCMobile.CustomControls
{
    public class DownloadButton : Button
    {
        public static readonly BindableProperty courseid =
               BindableProperty.Create("CourseID", typeof(string), typeof(DownloadButton));

        public static readonly BindableProperty spinner =
            BindableProperty.Create("Spinner", typeof(ActivityIndicator), typeof(DownloadButton));

        public static readonly BindableProperty launchButton =
            BindableProperty.Create("LaunchButton", typeof(Button), typeof(DownloadButton));

        public String CourseID
        {
            get { return (string)GetValue(courseid); }
            set { SetValue(courseid, value); }
        }

        public ActivityIndicator Spinner
        {
            get { return (ActivityIndicator)GetValue(spinner); }
            set { SetValue(spinner,value); }
        } 

        public Button LaunchButton
        {
            get { return (Button)GetValue(launchButton); }
            set { SetValue(launchButton, value); }
        }
    }
}
