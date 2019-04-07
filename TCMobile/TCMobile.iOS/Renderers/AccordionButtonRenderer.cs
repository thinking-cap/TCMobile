using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCMobile.iOS.Renderers;
using TCMobile.CustomControls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

using Foundation;
using UIKit;
[assembly: ExportRenderer(typeof(AccordionButton), typeof(AccordionButtonRenderer))]
namespace TCMobile.iOS.Renderers
{
    class AccordionButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            var view = (AccordionButton)Element;
            if (view == null) return;
        }

    }
}