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
                var docsDir = NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, null, true, out var error);
                var data = NSUrl.FromFilename(Path.Combine(docsDir.Path, Element.iOSPath));

                var content = NSUrl.FromFilename(Path.Combine(docsDir.Path, "Courses"));
                Control.LoadFileUrl(data, content);
                //string Base = Path.GetDirectoryName(Element.Uri) + "/*/";
                //var fileUrl = new NSUrl(Element.Uri, true);
                //var BaseUrl = new NSUrl(Base,true,fileUrl);
                //var BaseUrl = new NSUrl(Base, true, fileUrl);
                //Control.LoadRequest(new NSUrlRequest(fileUrl));
                //Control.LoadFileUrl(fileUrl,fileUrl.RemoveLastPathComponent());
                //Control.LoadHtmlString(Element.Source, fileUrl.RemoveLastPathComponent());
                //Control.LoadHtmlString(new NSString(Element.Source), BaseUrl);

            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeAction(message.Body.ToString());
        }
    }
}