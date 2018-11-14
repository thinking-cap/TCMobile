using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Catalogue : ContentPage
	{
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
                DisplayAlert("TC LMS", "File Saved Successfully", "Close");
            }
            else
            {
                DisplayAlert("TC LMS", "Error while saving the file", "Close");
            }
        }
        bool busy;
        async void DownloadClicked(object sender, EventArgs e)
        {
            if (busy)
                return;
            busy = true;
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
                downloader.DownloadFile("https://tcmagnum.blob.core.windows.net/domaincatalogue/00000000-0000-0000-0000-000000000000_domaincatalogue.json", "TCLMS");
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Access Denied", "Can not continue, try again.", "OK");
            }
            ((Button)sender).IsEnabled = true;
            busy = false;
        }
    }
}