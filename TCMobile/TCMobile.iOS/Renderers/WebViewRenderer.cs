using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin;
using TCMobile;
using Xamarin.Forms.Platform.iOS;

using Foundation;
using UIKit;
using TCMobile.iOS.Renderers;

[assembly: ExportRenderer(typeof(CustomWebview), typeof(CustomWebViewRenderer))]
namespace TCMobile.iOS.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            Delegate = new ExtendedUIWebViewDelegate(this);
                this.Opaque = false;
                this.BackgroundColor = UIColor.Clear;
          
        }
    }

    public class ExtendedUIWebViewDelegate : UIWebViewDelegate
    {
        CustomWebViewRenderer webViewRenderer;

        public ExtendedUIWebViewDelegate(CustomWebViewRenderer _webViewRenderer = null)
        {
            webViewRenderer = _webViewRenderer ?? new CustomWebViewRenderer();
        }

        public override async void LoadingFinished(UIWebView webView)
        {
            var wv = webViewRenderer.Element as CustomWebview;
            if (wv != null)
            {
                await System.Threading.Tasks.Task.Delay(100); // wait here till content is rendered
                wv.HeightRequest = (double)webView.ScrollView.ContentSize.Height;
            }
        }
    }
}