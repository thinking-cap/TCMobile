using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TCMobile;
using TCMobile.Droid;

using WebView = Android.Webkit.WebView;
[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]

namespace TCMobile.Droid
{
    public class ExtendedWebViewRenderer : ViewRenderer<ExtendedWebView, Android.Webkit.WebView>
    {
        static ExtendedWebView _xwebView = null;
        WebView _webView;
        Context _context;
        public ExtendedWebViewRenderer(Context context) : base(context)
        {
            _context = context;

        }
        class ExtendedWebViewClient : Android.Webkit.WebViewClient
        {
            public override async void OnPageFinished (WebView view, string url)
            {
                if (_xwebView != null) {
                    int i = 10;
                    while (view.ContentHeight == 0 && i-- > 0) // wait here till content is rendered
                        await System.Threading.Tasks.Task.Delay (100);
                    _xwebView.HeightRequest = view.ContentHeight;
                }
                base.OnPageFinished (view, url);
            }
        }
        

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedWebView> e)
        {
            base.OnElementChanged(e);
            _xwebView = e.NewElement as ExtendedWebView;
            _webView = Control;

            if (e.OldElement == null)
            {
                _webView.SetWebViewClient(new ExtendedWebViewClient());
            }

        }
    }
}