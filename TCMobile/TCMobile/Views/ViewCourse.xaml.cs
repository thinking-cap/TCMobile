using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewCourse : ContentPage
	{
		public ViewCourse (string courseid)
		{
			InitializeComponent ();
		}

        void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {
          //  labelLoading.IsVisible = true;
        }

        void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {
           // labelLoading.IsVisible = false;
        }
    }
}