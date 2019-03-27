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


                this.Opaque = false;
                this.BackgroundColor = UIColor.Clear;
          
        }
    }
}