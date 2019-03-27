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
            Models.Record courseExists = await App.Database.GetCourseByID(courseid);
            if(courseExists== null) { 
                Models.Record rec = new Models.Record();
                rec.CourseID = courseid;
                // find the course name
                rec.CourseName = catalogue.courses.Find(x => x.courseid == courseid).title;
                rec.Version = catalogue.courses.Find(x => x.courseid == courseid).version;
                rec.CourseDescription = catalogue.courses.Find(x => x.courseid == courseid).description;
                rec.CompletionStatus = "Not Started";
                rec.SuccessStatus = "";
                rec.Score = "";
                rec.Deleted = "false";

                rec.CMI = "";
                localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                await App.Database.SaveItemAsync(rec);
            }
            else
            {
                courseExists.Deleted = "false";
                await App.Database.SaveItemAsync(courseExists);
            }
        }

        private void OnFileProgress(object sender, DownloadProgress e)
        {

        }
        bool busy;
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string courseID = ((System.Net.WebClient)(sender)).QueryString["CourseID"];
            Unzip(courseID);
            getCMIObjectFromLMS(courseID);
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
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
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

            
            Currentdownload.IsVisible = false;
            StackLayout listViewItem = (StackLayout)Currentdownload.Parent;
            ActivityIndicator spinner = (ActivityIndicator)listViewItem.Children[2];
            spinner.IsVisible = false;
            Button launch = (Button)listViewItem.Children[1];
            launch.IsVisible = true;
           

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
                Crashes.TrackError(ex);
                string x = "failed";
            }
        }
        Button Currentdownload;
        async void DownloadClicked(object sender, EventArgs e)
        {

            if (busy)
                return;
            busy = true;
            Currentdownload = (Button)sender;
            Button button = (Button)sender;
            string id = button.ClassId;
            var courseObj = catalogue.courses.Where(course => course.courseid == id).FirstOrDefault();

            string version = button.Parent.ClassId;
            // disable the button to prevent double clicking
            ((Button)sender).IsEnabled = false;

           ((Button)sender).IsVisible= false;

            StackLayout listViewItem = (StackLayout)Currentdownload.Parent;
            ActivityIndicator spinner = (ActivityIndicator)listViewItem.Children[2];
            spinner.IsRunning = true;
            spinner.IsVisible = true;


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
           // ((Button)sender).Text = "Downloaded";
            busy = false;
        }

        public TCMobile.Catalogue catalogue;
        public TCMobile.LearningPaths lp;
      

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
                catalogue = await Courses.GetCatalogue(credentials.HomeDomain, credentials.UserID);
                lp = await Courses.GetLearningPaths(credentials.HomeDomain, credentials.UserID);
                buildCatalogue(catalogue.courses);
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
            
            foreach (Models.Record course in courses)
            {
                bool x = await buildCourseCard(course.CourseID, course.CourseName, course.CourseDescription);
            }
        }

        public async void buildCatalogue(List<Course> courses)
        {
            if (courses != null)
            {
                // CatalogueList.ItemsSource = catalogue.courses;
                CatalogueLoaded = true;
               
                foreach (Course course in courses)
                {
                    bool x = await buildCourseCard(course.courseid, course.title, course.description);
                }

            }
        }

        public async Task<bool> buildCourseCard(string courseid,string coursetitle,string coursedescription)
        {
            Frame frame;
            StackLayout layout;
            Button downloadBtn;
            Button launchBtn;
            ActivityIndicator spinner;
            Image marquee;
            Models.Record courseRecord = await App.Database.GetCourseByID(courseid);
            marquee = new Image
            {
                // lets cache the image so we for 72 hours //
                Source = new UriImageSource
                {
                    Uri = new Uri(Constants.BlobLocation + "/coursecontent/" + courseid + "/courselogo.gif"),
                    CacheValidity = new TimeSpan(72, 0, 0),
                    CachingEnabled = true
                }

            };
            frame = new Frame
            {
                HasShadow = true,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 0, 0, 8)
            };

            layout = new StackLayout
            {
                ClassId = "course_" + courseid

            };

            StackLayout cardBody = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 0),
                ClassId = "course_" + courseid,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Label title = new Label
            {
                Text = coursetitle,
                Style = (Style)Application.Current.Resources["headerStyle"]
            };

            //HtmlLabel description = new HtmlLabel
            //{
            //    Text = coursedescription
            //};

            string htmlText = @"<html>
                                    <head>
                                        <meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/>                                    
                                        <style type='text/css'>
                                             body{font-family:Segoe UI, Helvetica Neue,'Lucida Sans Unicode', Skia, sans-serif;
                                                    border:0px;padding:0px;margin:0px;
                                                    background-color:transparent;
                                                    overflow:hidden;
                                                }
                                        </style>    
                                    </head>
                                    <body>" + HttpUtility.HtmlDecode(coursedescription) + "</body></html>";
            var description = new CustomWebview
            {
                HeightRequest = 300,
                Source = new HtmlWebViewSource
                {
                    Html = htmlText
                },
                Style = (Style)Application.Current.Resources["descriptionWebView"]

            };

            




            downloadBtn = new Button
            {
                Text = "download",
                Image = "download.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = courseid,
                IsVisible = (courseRecord == null) ? true : (courseRecord.Deleted == "false") ? false : true
            };

            spinner = new ActivityIndicator
            {
                IsVisible = false,
                Style = (Style)Application.Current.Resources["spinnerStyle"],
                HeightRequest = 20
            };

            StackLayout cardFooter = new StackLayout
            {
                Padding = new Thickness(16, 0, 16, 8),
                ClassId = "course_" + courseid
            };

            launchBtn = new Button
            {
                Text = (courseRecord == null) ? "open" :
                        (courseRecord.CompletionStatus.ToLower() == "completed") ? "review" :
                        (courseRecord.CMI == "") ? "open" : "resume",
                IsVisible = (courseRecord == null) ? false : (courseRecord.Deleted == "false") ? true : false,
                Image = "launch_w.png",
                Style = (Style)Application.Current.Resources["buttonStyle"],
                ClassId = courseid

            };
            launchBtn.Clicked += launchCourse;
            downloadBtn.Clicked += DownloadClicked;
            cardBody.Children.Add(title);
            cardBody.Children.Add(description);
            layout.Children.Add(marquee);
            layout.Children.Add(cardBody);
            layout.Children.Add(cardFooter);
            cardFooter.Children.Add(launchBtn);
            cardFooter.Children.Add(downloadBtn);
            cardFooter.Children.Add(spinner);
            frame.Content = layout;
            Cat.Children.Add(frame);

            return true;
        }

        public void launchCourse(Object Sender, EventArgs args)
        {
            Button button = (Button)Sender;
            string id = button.ClassId;
            //StackLayout listViewItem = (StackLayout)button.Parent;            
            //Label label = (Label)listViewItem.Children[0];
           
            //String text = label.Text;
            Navigation.PushAsync(new ViewCourse(id));
        }  
        
        public void getCMIObjectFromLMS(string courseid)
        {
            API api = new API();
            dynamic cmi = api.GetCMIFromLMS(courseid);
        }
    }

   
}