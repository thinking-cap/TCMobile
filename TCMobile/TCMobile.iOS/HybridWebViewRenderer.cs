using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TCMobile;
using TCMobile.iOS;

using Foundation;
using UIKit;
using WebKit;
using System.IO;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace TCMobile.iOS
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                userController = new WKUserContentController();
                var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                userController.AddUserScript(script);
                userController.AddScriptMessageHandler(this, "invokeAction");

                var config = new WKWebViewConfiguration { UserContentController = userController };
                try
                {
                    config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("allowFileAccessFromFileURLs"));
                    config.Preferences.SetValueForKey(NSObject.FromObject(true), new NSString("allowUniversalAccessFromFileURLs"));
                }catch(Exception ex) { }
                var webView = new WKWebView(Frame, config);
                SetNativeControl(webView);
            }
            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                //string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Uri));
                //string fileName = string.Format($"file:///{0}", Element.Uri);
                //Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, true)));
                //Control.LoadRequest(new NSUrlRequest(new NSUrl($"file://{Element.Uri}", true)));
                Control.LoadFileUrl(new NSUrl(Element.Uri, true), NSUrl.CreateFileUrl(new[] { Path.GetDirectoryName(Element.Uri) }));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeAction(message.Body.ToString());
        }
    }
}