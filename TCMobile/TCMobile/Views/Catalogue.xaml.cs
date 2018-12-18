using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Newtonsoft.Json;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Catalogue : ContentPage
	{
        bool CatalogueLoaded = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(!CatalogueLoaded)
                LoadCourses();
        }

        
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public Catalogue ()
		{
			InitializeComponent ();
           downloader.OnFileDownloaded += OnFileDownloaded;
            
        }
        private void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                DisplayAlert("TC LMS", "File Saved Successfully " + e.FileDownloadMessage, "Close");
            }
            else
            {
                DisplayAlert("TC LMS", "Error while saving the file " + e.FileDownloadMessage, "Close");
            }
        }
        bool busy;
        async void DownloadClicked(object sender, EventArgs e)
        {
            if (busy)
                return;
            busy = true;

            Button button = (Button)sender;
            string id = button.ClassId;
            // disable the button to prevent double clicking
            ((Button)sender).IsEnabled = false;

            // let's check the permission
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            // if we don't have perissions let's ask
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await DisplayAlert("Need To Save Stuff", "Gunna need that permission", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                status = results[Permission.Storage];
            }

            if (status == PermissionStatus.Granted)
            {
                string url = "https://tcstable.blob.core.windows.net/coursepackages/" + id + "/1/CoursePackage.zip";

                downloader.DownloadFile(url, "TCLMS");
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Access Denied", "Can not continue, try again.", "OK");
            }
            ((Button)sender).IsEnabled = true;
            busy = false;
        }

        async void LoadCourses()
        {
            // show the spinner and turn it on 
            CatalogueProgress.IsVisible = true;
            CatalogueProgress.IsRunning = true;
            // Load the catalogue
            TCMobile.Catalogue catalogue = await Courses.GetCatalogue(Constants.ProgramID, Constants.StudentID);
            //Hide the spinner
            CatalogueProgress.IsVisible = false;
            CatalogueProgress.IsRunning = false;

            // Bind the courses to the ListView
            if (catalogue != null)
            {
                CatalogueList.ItemsSource = catalogue.courses;
                CatalogueLoaded = true;
            }

        }

        public void launchCourse(Object Sender, EventArgs args)
        {
            Button button = (Button)Sender;
            string id = button.ClassId;
            StackLayout listViewItem = (StackLayout)button.Parent;
            Label label = (Label)listViewItem.Children[0];

            String text = label.Text;
            Navigation.PushAsync(new ViewCourse(id));
        }
    }
}