using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Webkit;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace TCMobile.Droid
{
    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;
        bool apiloaded = false;
        public JavascriptWebViewClient(string javascript)
        {
            _javascript = javascript;
            
        }


        public override void OnLoadResource(WebView view, string url)
        {
            base.OnLoadResource(view, url);
           
            //view.EvaluateJavascript(_javascript, null);

        }
        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            //view.Visibility = ViewStates.Invisible;
            base.OnPageStarted(view, url, favicon);            
            
        }



        public override void OnPageFinished(WebView view, string url)
        {

           
            view.EvaluateJavascript(_javascript, null);          
            base.OnPageFinished(view, url);
            //view.Visibility = ViewStates.Visible;
        }
    }
}