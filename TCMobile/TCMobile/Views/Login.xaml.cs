using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Dynamic;

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

            string username;
            string password;

            username = Email.Text;
            password = Password.Text;

            TCMobile.LogInObj login = await userLogin.check(username, password);

            if (login.login == true)
            {
                App.IsUserLoggedIn = true;
                Application.Current.MainPage = new MainPage();
                Constants.StudentID = login.userId;
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                Error.Text = login.status;
            }
           
        }
    }
}