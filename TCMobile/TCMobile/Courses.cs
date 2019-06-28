using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using TCMobile.Views;
using Xamarin.Forms;
using Microsoft.AppCenter.Crashes;
using System.IO;
using System.Net;
using System.ComponentModel;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TCMobile.CustomControls;
using System.Linq;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace TCMobile
{
    class Courses
    {

        public async void CreateCourseRecord(string courseid, string cmi)
        {
            Models.Record courseExists = await App.Database.GetCourseByID(courseid);
            if (courseExists == null)
            {
                Models.Record rec = new Models.Record();
                rec.CourseID = courseid;
                // find the course name
                rec.CourseName = App.CourseCatalogue.courses.Find(x => x.courseid == courseid).title;
                rec.Version = App.CourseCatalogue.courses.Find(x => x.courseid == courseid).version;
                rec.CourseDescription = App.CourseCatalogue.courses.Find(x => x.courseid == courseid).description;
                rec.CompletionStatus = "Not Started";
                rec.SuccessStatus = "";
                rec.Score = "";
                rec.ScoreMax = "";
                rec.ScoreMin = "";
                rec.ScoreRaw = "";
                rec.Deleted = "false";
                rec.ProgressMeasure = "0";
                rec.DueDate = App.CourseCatalogue.courses.Find(x => x.courseid == courseid).duedate;
                rec.Synced = false;
                rec.CMI = cmi;
                rec.Downloaded = true;
                App.LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                await App.Database.SaveItemAsync(rec);
            }
            else
            {
                courseExists.Deleted = "false";
                courseExists.Downloaded = true;
                if (String.IsNullOrEmpty(courseExists.CMI))
                    courseExists.CMI = cmi;
                if (String.IsNullOrEmpty(courseExists.ScoreMax))
                {
                    courseExists.ScoreMax = "";
                    courseExists.ScoreMin = "";
                    courseExists.ScoreRaw = "";
                }
                await App.Database.SaveItemAsync(courseExists);
            }
        }

        public async void Unzip(string CourseID, string cmi)
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
            Courses courses = new Courses();
            courses.CreateCourseRecord(CourseID,cmi);

            closePopup();
            var action = await Application.Current.MainPage.DisplayAlert("Finished", "Would you like to launch the course?", "Yes", "No");
            //DisplayAlert("Finished", "Course had successfully been download.", "OK");
           // Debug.WriteLine("action " + action);
            if (action)
            {
                string test = await Courses.openCourse(CourseID, Application.Current.MainPage.Navigation);
            }


            App.Currentdownload.IsVisible = false;

            App.Currentdownload.Spinner.IsVisible = false;
            App.Currentdownload.LaunchButton.IsVisible = true;
            App.Currentdownload.BtnLabel.Text = "Open";
            App.Currentdownload.BtnLabel.IsVisible = true;


        }


        bool busy = false;
        public async void DownloadClicked(object sender, EventArgs e)
        {
            var profiles = Connectivity.ConnectionProfiles;
            var current = Connectivity.NetworkAccess;
            bool allowDownload = true;
            if (current == NetworkAccess.Internet)
            {
                // allow download only if the internet only == false
                allowDownload = (Constants.WifiOnly == "True" && profiles.Contains(ConnectionProfile.WiFi) == false) ? false : true;
            }
            else
            {
                // don't ever try to download with no connection
                allowDownload = false;
            }
                if (allowDownload) { 
                dynamic button;
                if (busy)
                    return;
                busy = true;
                if (sender is DownloadButton)
                {
                    App.Currentdownload = (DownloadButton)sender;
                    button = (DownloadButton)sender;
                }
                else
                {
                    App.Currentdownload = (DownloadImageButton)sender;
                    button = (DownloadImageButton)sender;
                }
           
                string id = button.CourseID;
                var courseObj = App.CourseCatalogue.courses.Where(course => course.courseid == id).FirstOrDefault();

                string version = button.Parent.ClassId;
                // disable the button to prevent double clicking
                button.IsEnabled = false;
                button.IsVisible = false;

               // button.Spinner.IsRunning = true;
               // button.Spinner.IsVisible = true;
                button.BtnLabel.IsVisible = false;


                // let's check the permission
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                // if we don't have perissions let's ask
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await Application.Current.MainPage.DisplayAlert("Need To Save Stuff", "Gunna need that permission", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    string url = Constants.Url + "/mobile/GetCourse.ashx?CourseID=" + id + "&Version=1";
                    //string url = "https://tcstable.blob.core.windows.net/coursepackages/" + id + "/" + version + "/CoursePackage.zip";
                    Download(url, "TCLMS/Temp", id);
                    //downloader.DownloadFile(url, "TCLMS/Temp",id);
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await Application.Current.MainPage.DisplayAlert("Access Denied", "Can not continue, try again.", "OK");
                }
                // ((Button)sender).Text = "Downloaded";
                busy = false;
            }
            else
            {
                if (current == NetworkAccess.Internet)
                    await Application.Current.MainPage.DisplayAlert("Warning", "You have selected to only download over WiFi. Connect to WiFi or go To Settings and change your preference.", "OK");
                else
                    await Application.Current.MainPage.DisplayAlert("Warning", "You must connect to a network to download content", "OK");
            }
        }
        public Pages.LoadingPage _downloadPage = new Pages.LoadingPage("Downloading");
        private async void openPopup(string msg)
        {
           // _downloadPage = new Pages.LoadingPage(msg);
            await PopupNavigation.Instance.PushAsync(_downloadPage);
        }

        private async void closePopup()
        {

            if (_downloadPage != null)
                await PopupNavigation.Instance.PopAllAsync(true);
        }

        void Download(string url, string folder, string id)
            {
            
            try
                {
                    openPopup("Downloading");
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

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("Downloaded {0}mbs",
                ((int)e.BytesReceived / 1000000)
               );

        }

        private async void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string courseID = ((System.Net.WebClient)(sender)).QueryString["CourseID"];
            Courses courses = new Courses();
            Models.Record courseExists = await App.Database.GetCourseByID(courseID);
            string cmi;
            if (courseExists == null)
                cmi = await getCMIObjectFromLMS(courseID);
            else
                cmi = "";
            courses.Unzip(courseID, cmi);
            
        }




        public void launchCourse(Object Sender, EventArgs args)
        {
            DownloadImageButton button = (DownloadImageButton)Sender;
            

            //String text = label.Text;
            Page p = ViewExtensions.GetParentPage(button);
            p.Navigation.PushModalAsync(new ViewCourse(button.CourseID));
        }
        public async Task<string> getCMIObjectFromLMS(string courseid)
        {
            API api = new API();
            string cmi = await api.GetCMIFromLMS(courseid);
            return cmi;
            //TODO need to write the cmi object to the db if possible//
        }

        public static async Task<Catalogue> GetCatalogue(string domainid,string studentid)
        {
            try
            {
                string uri = Constants.StudentCatalogue + "?studentid=" + studentid + "&programid=" + domainid;
                dynamic results = await DataService.getDataFromService(uri).ConfigureAwait(false);


                if (results.courses != null)
                {
                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
            
           
        }

        public static async Task<LPS> GetLearningPaths(string domainid, string studentid)
        {
            try
            {
                string uri = Constants.LearningPaths + "?studentid=" + studentid + "&programid=" + domainid;
                dynamic results = await DataService.getLPs(uri).ConfigureAwait(false);
                return results;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<bool> SaveMap(string lpid, StudentActivityMap activitymap)
        {
            
            Models.LPDBRecord learningPath = await App.Database.GetLPByID(lpid);
            if (learningPath != null && learningPath.LPMap == "")
            {
                string map = JsonConvert.SerializeObject(activitymap);
                learningPath.LPMap = map;
                await App.Database.SaveLPAsync(learningPath);
            }
            return true;
        }

        public static async Task<Map> GetLearningPath(string domainid, string studentid, string lpid)
        {
            try
            {
                string uri = Constants.LearningPath + "?studentid=" + studentid + "&programid=" + domainid +"&lpid=" + lpid;
                dynamic results = await DataService.getLPDetails(uri).ConfigureAwait(false);
                return results;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static async Task<String>openCourse(string id, INavigation navigation)
        {
            //MainPage.Navigation.PushAsync(new ViewCourse(id));
            Page p = ViewExtensions.GetParentPage(App.Currentdownload);
            await p.Navigation.PushModalAsync(new ViewCourse(id));

            return "";
           
        }



        public async Task<List<Models.Record>>CheckForCourses()
        {

            List<Models.Record> courses = await App.Database.GetItemsAsync();
            var sorted = courses.OrderBy(o => o.CourseName).ToList();
            return sorted;
        }

        public async Task<Models.LPDBRecord>GetActivityMap(string id)
        {
            Models.LPDBRecord lp = await App.Database.GetLPByID(id);
            return lp;
        }

        public async Task<List<Models.LPDBRecord>> CheckForLPS()
        {
            List<Models.LPDBRecord> lps = await App.Database.GetLPSAsync();

            return lps;

        }
    }
}
