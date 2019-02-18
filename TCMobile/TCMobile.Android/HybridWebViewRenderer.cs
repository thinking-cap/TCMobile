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
using Mono;


[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace TCMobile.Droid
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>
    {

        const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        Context _context;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }
        
      
        

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                String API = Element.APIJS;
                
                var webView = new Android.Webkit.WebView(_context);
                webView.Settings.AllowContentAccess = true;
                webView.Settings.AllowUniversalAccessFromFileURLs = true;
                webView.Settings.AllowFileAccessFromFileURLs = true;
                webView.Settings.AllowContentAccess = true;
                webView.Settings.AllowFileAccess = true;
                webView.Settings.JavaScriptEnabled = true;                
                webView.Settings.AllowFileAccessFromFileURLs = true;
                webView.VerticalScrollBarEnabled = true;
               
                webView.SetWebViewClient(new JavascriptWebViewClient($"javascript:{JavascriptFunction}"));
                webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {API}"));

                
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
               
                String API = Element.APIJS;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.Settings.AllowContentAccess = true;
                Control.Settings.AllowFileAccess = true;
                Control.Settings.JavaScriptEnabled = true;
                Control.Settings.AllowFileAccessFromFileURLs = true;
                Control.VerticalScrollBarEnabled = true;
                //Control.SetWebViewClient(new JavascriptWebViewClient($"javascript: {API}"));
                
                Control.LoadUrl($"file:///{Element.Uri}");
                //Control.LoadData(Element.Source, "text/html", "UTF-8");
                //Control.LoadDataWithBaseURL(Element.BaseUrl, Element.Source, "text/html", "UTF-8", null);
            }
        }
    }
}