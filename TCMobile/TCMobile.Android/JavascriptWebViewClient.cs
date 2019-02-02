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

namespace TCMobile.Droid
{
    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;

        public JavascriptWebViewClient(string javascript)
        {
            _javascript = javascript;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }
    }
}