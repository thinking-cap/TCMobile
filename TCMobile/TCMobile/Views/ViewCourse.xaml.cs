using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewCourse : ContentPage
	{
        public static HybridWebView courseWindow;
       
        public ViewCourse (string courseid)
		{
			InitializeComponent ();
            string courseindex = "Courses/2d7d0a7d-145a-41d0-9abf-685a2b5dfc3c/Online_Placement_Test_no_timer_pack/YKZOP4NACH3EPJNTG6M4T2BQDI/Unit_4_5/995/Unit.html";
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string coursePath = Path.Combine(localFolder, courseindex);
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ViewCourse)).Assembly;
            //Stream stream = assembly.GetManifestResourceStream("TCMobile.course.htm");
            String baseUrl = "file:/" + Path.GetDirectoryName(coursePath);
            FileStream stream = File.OpenRead(coursePath);
            if (stream == null)
            {
                throw new InvalidOperationException(
                    String.Format("Cannot create stream from specified URL: {0}", "course.htm"));
            }

            string iframe = @"<html><head>
                                    <script type='text/javascript'>
                                            var API_1484_11 ={
                                                Initialize : function(){return 'true';}
                                            };
                                    </script>
                                    <style type='text/css'>
                                        iframe{width:100%;height:100%;border:0px;}
                                    </style>
                                </head><body>
                                <iframe width='100%' height='100%' src='" + coursePath+"'></iframe></body></html>";
            StreamReader reader = new StreamReader(stream);
            string htmlString = reader.ReadToEnd();

            courseWindow = new HybridWebView
            {
                HeightRequest = 1000,
                WidthRequest = 1000,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "TCMobile_CourseView"
               
                

            };

            
           
            WebViewContainer.Children.Add(courseWindow);

            HtmlWebViewSource html = new HtmlWebViewSource();
            //string baseurl = html.BaseUrl;
            // html.Html = htmlString;
            html.Html = htmlString;
            //  html.BaseUrl = DependencyService.Get<iBaseURL>().Get();
            html.BaseUrl = baseUrl;
            //courseWindow.Source = iframe;
            courseWindow.Uri = coursePath;
            
            
           // courseWindow.Navigating += webviewNavigating;

            

        }

        void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            //  labelLoading.IsVisible = true;
            string cmd = e.Url;
            //e.Cancel = true;
        }

        void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {
           // labelLoading.IsVisible = false;
        }

        public static string retorno;
        //public static async Task JSRun()
        //{
        //    retorno = await courseWindow.EvaluateJavaScriptAsync("getState();");
        //}
    }
}