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
        ICredentialsService storeService;
        
        public Login ()
		{

            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
            if (doCredentialsExist)
            {
                App.CredentialsService.DeleteCredentials();
                App.IsUserLoggedIn = false;

            }
        }
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
           
            string username;
            string password;
            username = Email.Text;
            password = Password.Text;
            Progress.IsVisible = true;
            Progress.IsRunning = true;
            Error.Text = "";
            TCMobile.LogInObj login = await userLogin.check(username, password);

            if (login.login == true)
            {
                bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
                if (!doCredentialsExist)
                {
                    App.CredentialsService.SaveCredentials(username, password,login.userId);
                }

                App.IsUserLoggedIn = true;
                Application.Current.MainPage = new MainPage();
                Constants.StudentID = login.userId;
                Constants.firstName = login.firstName;
                Constants.lastName = login.lastName;
                Progress.IsVisible = true;
                Progress.IsRunning = true;

                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                Error.Text = login.status;
                Progress.IsVisible = false;
                Progress.IsRunning = false;
            }
           
        }
    }
}