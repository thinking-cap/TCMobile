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
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
		}
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            
            
                App.IsUserLoggedIn = true;
                Application.Current.MainPage = new MainPage();
                await Navigation.PushAsync(new MainPage());
           
        }
    }
}