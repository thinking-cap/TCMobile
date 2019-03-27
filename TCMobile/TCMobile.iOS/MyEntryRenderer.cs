using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using TCMobile;
using TCMobile.iOS;


[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace TCMobile.iOS
{
    class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.BackgroundColor = UIColor.FromRGB(255, 255, 255);
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
    
}

