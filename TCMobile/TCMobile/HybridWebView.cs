using System;
using Xamarin.Forms;

namespace TCMobile
{
    public class ExtendedWebView : WebView { }
    public class HybridWebView : View
    {
        Action<string> action;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(
            propertyName: "Source",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public static readonly BindableProperty CMIProperty = BindableProperty.Create(
            propertyName: "CMI",
            returnType: typeof(string),
            declaringType: typeof(HybridWebView),
            defaultValue: default(string));

        public static readonly BindableProperty BaseUrlProperty = BindableProperty.Create(
           propertyName: "BaseUrl",
           returnType: typeof(string),
           declaringType: typeof(HybridWebView),
           defaultValue: default(string));

        public static readonly BindableProperty iOSPathProperty = BindableProperty.Create(
           propertyName: "iOSPath",
           returnType: typeof(string),
           declaringType: typeof(HybridWebView),
           defaultValue: default(string));

        public static readonly BindableProperty APIProperty = BindableProperty.Create(
          propertyName: "API",
          returnType: typeof(string),
          declaringType: typeof(HybridWebView),
          defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public string BaseUrl
        {
            get { return (string)GetValue(BaseUrlProperty); }
            set { SetValue(BaseUrlProperty, value); }
        }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public string CMI
        {
            get { return (string)GetValue(CMIProperty); }
            set { SetValue(CMIProperty, value); }
        }

        public string APIJS
        {
            get { return (string)GetValue(APIProperty); }
            set { SetValue(APIProperty, value); }
        }

        public string iOSPath
        {
            get { return (string)GetValue(iOSPathProperty); }
            set { SetValue(iOSPathProperty, value); }
        }

        public void RegisterAction(Action<string> callback)
        {
            action = callback;
        }

        public void Cleanup()
        {
            action = null;
        }

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }
    }
}
