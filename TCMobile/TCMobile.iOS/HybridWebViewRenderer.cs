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
        const string API = "function SetupAPI(){API_1484_11={Initialize: function(){window.webkit.messageHandlers.invokeAction.postMessage('hell');return 'true';}}}";
        const string APISetup= "(function(){SetupAPI();})()";
        const string Meta = @"
                var meta = document.createElement('meta');
                meta.setAttribute('name', 'viewport');
                meta.setAttribute('content', 'width=device-width');
                meta.setAttribute('initial-scale', '1.0');
                meta.setAttribute('maximum-scale', '1.0');
                meta.setAttribute('minimum-scale', '1.0');
                meta.setAttribute('user-scalable', 'no');
                document.getElementsByTagName('head')[0].appendChild(meta);
        ";

        WKUserContentController userController;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                userController = new WKUserContentController();
                var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, true);
                var api = new WKUserScript(new NSString(API), WKUserScriptInjectionTime.AtDocumentStart, true);
                var apiSetup = new WKUserScript(new NSString(APISetup), WKUserScriptInjectionTime.AtDocumentStart, true);
                userController.AddUserScript(script);
                userController.AddUserScript(api);
                userController.AddUserScript(apiSetup);

                userController.AddScriptMessageHandler(this, "invokeAction");
               
               
                var config = new WKWebViewConfiguration { UserContentController = userController };
                WKJavascriptEvaluationResult handler = (NSObject result, NSError err) => {
                    if (err != null)
                    {
                        System.Console.WriteLine(err);
                    }
                    if (result != null)
                    {
                        System.Console.WriteLine(result);
                    }
                };
                var webView = new WKWebView(Frame, config);
                var js = (NSString)"SetupAPI()";
                webView.EvaluateJavaScript(js, null);
                webView.CustomUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_1_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1";
                webView.SizeToFit();

                
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
                var frame = NSUrl.FromFilename(Path.Combine(docsDir.Path, "Courses/index.htm"));
                var htmlString = new NSString(Element.Source);
                var content = NSUrl.FromFilename(Path.Combine(docsDir.Path, "Courses"));
                var contentFrame = NSUrl.FromFilename(Path.Combine(docsDir.Path));
                Control.LoadFileUrl(data, content);

               




            }
        }

        

        [Foundation.Export("webView:decidePolicyForNavigationResponse:decisionHandler:")]
        public virtual void DecidePolicy(WKWebView webView, WKNavigationResponse navigationResponse, Action<WKNavigationResponsePolicy> decisionHandler)
        {
            //NSHttpUrlResponse response;
            //NSHttpCookie[] cookies_holder;

            //response = (NSHttpUrlResponse)navigationResponse.Response;

            //cookies_holder = NSHttpCookie.CookiesWithResponseHeaderFields(response.AllHeaderFields, response.Url);

            //foreach (var cookie in cookies_holder)
            //{
            //    NSHttpCookieStorage.SharedStorage.SetCookie(cookie);
            //}

            decisionHandler(WKNavigationResponsePolicy.Allow);
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeAction(message.Body.ToString());
        }

        public void APIResult(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeAction(message.Body.ToString());
        }
    }
}