using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using TCMobile.Droid;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AppCenter.Crashes;


[assembly: Dependency(typeof(AndroidDownloader))]
namespace TCMobile.Droid
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;
        public event EventHandler<DownloadProgress> OnFileProgress;
        private string CourseID;
        public void DownloadFile(string url, string folder, string courseid)
        {
            CourseID = courseid; 
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, folder);
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();                
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName("CoursePackage.zip"));
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(ex.Message,false));
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(e.Error.Message,false));
            }
            else
            {
                Unzip();
            }
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            
            Console.WriteLine("Downloaded {0}mbs",
                ((int)e.BytesReceived/1000000)
               );
            string percentage = ((int)e.BytesReceived / 1000000).ToString();
            if (OnFileProgress != null)
            {
                OnFileProgress.Invoke(this,new DownloadProgress(percentage));
            }
        }

        private void Unzip()
        {
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "TCLMS/Temp/CoursePackage.zip");
            string newFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "TCLMS/Courses/" + CourseID);
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
                catch(Exception ex) {
                    Crashes.TrackError(ex);
                }

            }
            File.Delete(pathToNewFolder);

            
            if (OnFileDownloaded != null)
                OnFileDownloaded.Invoke(this, new DownloadEventArgs("Course Downloaded", true,CourseID));
        }
       
           // var Zip = new GZipStream(System.IO.File.Open(pathToNewFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None),1)
    }
}