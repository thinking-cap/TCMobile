using System;
using System.Threading.Tasks;
using TCMobile.iOS.Services;
using TCMobile.Services;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(StorageiOS))]

namespace TCMobile.iOS.Services
{
    public class StorageiOS : iStorage
    {
        public Task<UInt64> GetFreeSpace()
        {
            // var personalFolder = NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            NSFileSystemAttributes applicationFolder = NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            var freeSpace = applicationFolder.FreeSize;
            var totalSpace = applicationFolder.Size;
            return Task.FromResult(applicationFolder.FreeSize);
        }
    }
}