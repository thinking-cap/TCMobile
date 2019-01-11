using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCMobile
{
    public interface IDownloader
    {

        // interface for app specific download methods
        void DownloadFile(string url, string folder,string courseid);
        event EventHandler<DownloadEventArgs> OnFileDownloaded;
        event EventHandler<DownloadProgress> OnFileProgress;
    }

    
    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved = false;
        public string FileDownloadMessage = "";
        public string FilePercent = "";
       
        public DownloadEventArgs(string errorMessage, bool fileSaved)
        {
            FileDownloadMessage = errorMessage;
            FileSaved = fileSaved;
        }


    }

    public class DownloadProgress : EventArgs
    {
       
        public string FilePercent = "";

       
        public DownloadProgress(string percent)
        {
            FilePercent = percent;
        }


    }






}
