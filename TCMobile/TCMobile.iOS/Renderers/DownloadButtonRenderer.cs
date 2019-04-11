using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCMobile.iOS.Renderers;
using TCMobile.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DownloadButton), typeof(DownloadButtonRenderer))]
namespace TCMobile.iOS.Renderers
{   
        class DownloadButtonRenderer : ButtonRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
            {
                base.OnElementChanged(e);
                var view = (DownloadButton)Element;
                if (view == null) return;
            }

        }   
}