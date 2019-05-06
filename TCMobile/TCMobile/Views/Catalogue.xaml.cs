using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.ComponentModel;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using Xamarin.Essentials;
using XamForms.HtmlLabel;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using System.Web;
using TCMobile.CustomControls;


namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Catalogue : ContentPage
	{
        bool CatalogueLoaded = false;
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Constants.deviceWidth = this.Width;
            NavigationPage.SetHasNavigationBar(this, false);
            LoadCourses();
        }

        
        IDownloader downloader = DependencyService.Get<IDownloader>();
        public Catalogue ()
		{
			InitializeComponent ();
            downloader.OnFileDownloaded += OnFileDownloaded;
            downloader.OnFileProgress += OnFileProgress;

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
       

        private void OnFileProgress(object sender, DownloadProgress e)
        {

        }
       
        

       

        


        
        
      

        
      

        async void LoadCourses()
        {
            Cat.Children.Clear();
            // show the spinner and turn it on 
            CatalogueProgress.IsVisible = true;
            CatalogueProgress.IsRunning = true;
            // Load the catalogue
            CredentialsService credentials = new CredentialsService();
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet) {
                App.CourseCatalogue = await Courses.GetCatalogue(credentials.HomeDomain, credentials.UserID);                
                buildCatalogue(App.CourseCatalogue.courses);
            }
            else
            {
                buildCatalogueOffline();
            }
            //Hide the spinner
            CatalogueProgress.IsVisible = false;
            CatalogueProgress.IsRunning = false;

            // Bind the courses to the ListView
            

        }

        public async void buildCatalogueOffline()
        {
            Courses c = new Courses();
            List<Models.Record> courses = await c.CheckForCourses();
            
            CatalogueLoaded = true;
            Cards card = new Cards();
            Courses click = new Courses();
            foreach (Models.Record course in courses)
            {
                bool x = await card.buildCourseCard(course.CourseID, course.CourseName, course.CourseDescription,Cat,click.DownloadClicked, click.launchCourse,course.DueDate);
            }
        }

        

        

        public async void buildCatalogue(List<Course> courses)
        {
            if (courses != null)
            {
                // CatalogueList.ItemsSource = catalogue.courses;
                CatalogueLoaded = true;
                Cards card = new Cards();
                Courses click = new Courses();
                foreach (Course course in courses)
                {
                    bool x = await card.buildCourseCard(course.courseid, course.title, course.description,Cat,click.DownloadClicked,click.launchCourse,course.duedate);
                }

            }
        }

       

       
        
       
    }

   
}