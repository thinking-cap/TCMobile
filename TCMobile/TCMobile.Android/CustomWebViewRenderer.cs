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
using TCMobile;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TCMobile.Droid;


[assembly: ExportRenderer(typeof(CustomWebview), typeof(TransparentWebViewRenderer))]
namespace TCMobile.Droid
{
    
    
    class TransparentWebViewRenderer : WebViewRenderer
    {

        public TransparentWebViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}