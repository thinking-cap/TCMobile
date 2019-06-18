using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using System.Net;
using Refractored.XamForms.PullToRefresh.Droid;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using FFImageLoading.Forms.Platform;

namespace TCMobile.Droid
{
    [Activity(Label = "TCMobile", Icon = "@drawable/icon",    Theme = "@style/MainTheme",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                   (sender, cert, chain, sslPolicyErrors) => true;
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CrossCurrentActivity.Current.Init(this,savedInstanceState);
            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            CachedImageRenderer.Init(enableFastRenderer:true);

            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false
            };


            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
            
            AppCenter.Start("6f839c85-8c8a-4e6a-89ac-d4b82b3d49b1",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("6f839c85-8c8a-4e6a-89ac-d4b82b3d49b1", typeof(Analytics), typeof(Crashes));

       
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}