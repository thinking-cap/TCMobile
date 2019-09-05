using System;
using System.Collections.Generic;
using System.Linq;
using Refractored.XamForms.PullToRefresh.iOS;
using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Distribute;

namespace TCMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            LoadApplication(new App());
            UIApplication.SharedApplication.StatusBarHidden = true;
            AppCenter.Start("35d5cf8e-f79c-4e64-9390-d7169eda5555", typeof(Analytics), typeof(Crashes));
            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

            return base.FinishedLaunching(app, options);
        }
    }
}
