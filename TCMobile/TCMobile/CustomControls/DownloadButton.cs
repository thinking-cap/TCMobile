using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

/*****************************************************************************
 *Two types of buttons here. A standard button and an Image button.
 * Both have the same properties use which ever you need 
 * CourseID,Spinner (the ref to the activity spinner) and 
 * LaunchButton (the button you switch to after the download has happened) 
 ***************************************************************************/

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

    public class DownloadImageButton : ImageButton
    {
        public static readonly BindableProperty courseid =
               BindableProperty.Create("CourseID", typeof(string), typeof(DownloadImageButton));

        public static readonly BindableProperty btnlabel =
            BindableProperty.Create("BtnLabel", typeof(Label), typeof(DownloadImageButton));

        public static readonly BindableProperty spinner =
            BindableProperty.Create("Spinner", typeof(ActivityIndicator), typeof(DownloadImageButton));

        public static readonly BindableProperty launchButton =
            BindableProperty.Create("LaunchButton", typeof(ImageButton), typeof(DownloadImageButton));

        public String CourseID
        {
            get { return (string)GetValue(courseid); }
            set { SetValue(courseid, value); }
        }

        public Label BtnLabel
        {
            get { return (Label)GetValue(btnlabel); }
            set { SetValue(btnlabel, value); }
        }

        public ActivityIndicator Spinner
        {
            get { return (ActivityIndicator)GetValue(spinner); }
            set { SetValue(spinner, value); }
        }

        public ImageButton LaunchButton
        {
            get { return (ImageButton)GetValue(launchButton); }
            set { SetValue(launchButton, value); }
        }
    }
}
