﻿using System;
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
        private string localFolder;
        async void CreateCourseRecord(string courseid)
        {
            Models.Record rec = new Models.Record();
            rec.CourseID = courseid;
            // find the course name
            rec.CourseName = catalogue.courses.Find(x => x.courseid == courseid).title;
            rec.Version = catalogue.courses.Find(x => x.courseid == courseid).version;
            localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            await App.Database.SaveItemAsync(rec);

        }

        private void OnFileProgress(object sender, DownloadProgress e)
        {

        }
        bool busy;
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string courseID = ((System.Net.WebClient)(sender)).QueryString["CourseID"];
            Unzip(courseID);
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("Downloaded {0}mbs",
                ((int)e.BytesReceived / 1000000)
               );

        }

        async void Unzip(string CourseID)
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string pathToNewFolder = Path.Combine(localFolder, "CoursePackage.zip");
            string newFolder = Path.Combine(localFolder, "Courses/" + CourseID);
            string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName("CoursePackage.zip"));

            ZipFile zf = null;
            FileStream fs = File.OpenRead(pathToNewFolder);
            zf = new ZipFile(fs);

            foreach (ZipEntry zipEntry in zf)
            {
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];     // 4K is optimum
                Stream zipStream = zf.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(newFolder, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                try
                {
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);

                    }
                }
                catch
                {

                }

            }


            File.Delete(pathToNewFolder);
            CreateCourseRecord(CourseID);
            var action = await DisplayAlert("Finished", "Would you like to launch the course?", "Yes", "No");
            //DisplayAlert("Finished", "Course had successfully been download.", "OK");
            Debug.WriteLine("action " + action);
            if (action)
            {
                string test = await Courses.openCourse(CourseID, Navigation);
            }


        }


        void Download(string url, string folder, string id)
        {
            
            try
            {
                WebClient webClient = new WebClient();
                webClient.QueryString.Add("CourseID", id);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                string pathToNewFile = Path.Combine(Constants.LocalFolder, Path.GetFileName("CoursePackage.zip"));
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            }
            catch (Exception ex)
            {
                string x = "failed";
            }
        }
       
        async void DownloadClicked(object sender, EventArgs e)
        {

            if (busy)
                return;
            busy = true;

            Button button = (Button)sender;
            string id = button.ClassId;
            var courseObj = catalogue.courses.Where(course => course.courseid == id).FirstOrDefault();

            string version = button.Parent.ClassId;
            // disable the button to prevent double clicking
            ((Button)sender).IsEnabled = false;

            ((Button)sender).Text= "Downloading";

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
               
                string url = Constants.Url + "/mobile/GetCourse.ashx?CourseID=" + courseObj.courseid + "&Version=" + courseObj.version;
                //string url = "https://tcstable.blob.core.windows.net/coursepackages/" + id + "/" + version + "/CoursePackage.zip";
                Download(url, "TCLMS/Temp", id);
                //downloader.DownloadFile(url, "TCLMS/Temp",id);
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Access Denied", "Can not continue, try again.", "OK");
            }
            ((Button)sender).Text = "Downloaded";
            busy = false;
        }

        public TCMobile.Catalogue catalogue;
       


        async void LoadCourses()
        {
            // show the spinner and turn it on 
            CatalogueProgress.IsVisible = true;
            CatalogueProgress.IsRunning = true;
            // Load the catalogue
            CredentialsService credentials = new CredentialsService();
            
            catalogue = await Courses.GetCatalogue(credentials.HomeDomain, credentials.UserID);
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