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
using WebView = Android.Webkit.WebView;


[assembly: ExportRenderer(typeof(CustomWebview), typeof(TransparentWebViewRenderer))]
namespace TCMobile.Droid
{
    
    
    class TransparentWebViewRenderer : WebViewRenderer
    {
        static CustomWebview myweb = null;
        WebView web;
        public TransparentWebViewRenderer(Context context) : base(context)
        {
        }
        class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {
            public override async void OnPageFinished(WebView view, string url)
            {
                if (myweb != null)
                {
                    int i = 10;
                    while (view.ContentHeight == 0 && i-- > 0)
                        await System.Threading.Tasks.Task.Delay(100);
                    myweb.HeightRequest = view.ContentHeight;
                }
                base.OnPageFinished(view, url);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
                base.OnElementChanged(e);
            myweb = e.NewElement as CustomWebview;
                web = Control;

                if (e.OldElement == null)
                {
                    web.SetWebViewClient(new ExtendedWebViewClient());
                }

                web.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }

       
    }
}