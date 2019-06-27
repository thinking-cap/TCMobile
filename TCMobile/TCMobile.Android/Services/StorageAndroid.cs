using System;
using System.Threading.Tasks;
using Android.OS;
using TCMobile.Droid.Services;
using TCMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(StorageAndroid))]
namespace TCMobile.Droid.Services
{
    public class StorageAndroid : iStorage
    {
        public Task<UInt64> GetFreeSpace()
        {
            var fullExternalStorage = Android.OS.Environment.ExternalStorageDirectory.TotalSpace;
            var freeExternalStorage = Android.OS.Environment.ExternalStorageDirectory.UsableSpace;

            var fullInternalStorage = Android.OS.Environment.RootDirectory.TotalSpace;
            var freeInternalStorage = Android.OS.Environment.RootDirectory.UsableSpace;



            //Using StatFS
            var path = new StatFs(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData));
            long blockSize = path.BlockSizeLong;
            long avaliableBlocks = path.AvailableBlocksLong;
            var produto = (blockSize * avaliableBlocks).ToString();
            return Task.FromResult(UInt64.Parse(produto));
        }
    }
}