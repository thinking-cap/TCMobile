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
	public partial class ForgotPassword : ContentPage
	{
		public ForgotPassword ()
		{
			InitializeComponent ();
		}

        private void Close_Clicked(object sender, EventArgs e)
        {
            // Device.BeginInvokeOnMainThread(async () => await api.CommitToLMS(CMIString, courseid));
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync());
        }
    }
}