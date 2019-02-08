using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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
            string launch = itemPath(courseid);

            //string courseindex = "Courses/2d7d0a7d-145a-41d0-9abf-685a2b5dfc3c/YKZOP4NACH3EPJNTG6M4T2BQDI/Unit_4_5/995/Unit.html";
            string courseindex = "Courses/" + courseid + "/" + launch;
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

            string iframe = "<html><head>" +
                                    "<script type='text/javascript'> " +
                                            "var API_1484_11 ={" +
                                                "Initialize : function(){return 'true'; " +
                                            "};" +                                            
                                    "</script>" +
                                    "<style type='text/css'>" +
                                        "iframe{width:100%;height:100%;border:0px;}" +
                                    "</style>" +
                                "</head><body>" +
                                "<iframe width='100%' id='coursewindow' height='100%'></iframe>" +
                                "<script type='text/javascript'>document.getElementById('coursewindow').src = 'https://thinkingcap.com';" + "</script>" +
                                "</body></html>";
            StreamReader reader = new StreamReader(stream);

            using (FileStream f = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Courses/index.htm")))
            {
                using (StreamWriter w = new StreamWriter(f, Encoding.UTF8))
                {
                    w.WriteLine(iframe);
                }
            }
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
            courseWindow.Source = htmlString;
            courseWindow.Uri = coursePath;
            courseWindow.iOSPath = courseindex;
            
            
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

        public string itemPath(string courseid)
        {
            XNamespace ns = "http://www.imsglobal.org/xsd/imscp_v1p1";
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string manifestXML = Path.Combine(localFolder, "Courses/" + courseid + "/imsmanifest.xml");
            // XDocument manifest = XDocument.Load(manifestXML);


            XmlDocument manifest = new XmlDocument();
            manifest.Load(manifestXML);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(manifest.NameTable);
            nsmgr.AddNamespace("imsss", "http://www.imsglobal.org/xsd/imsss");
            nsmgr.AddNamespace("adlseq", "http://www.adlnet.org/xsd/adlseq_v1p3");
            nsmgr.AddNamespace("mn", manifest.DocumentElement.NamespaceURI);
            XmlNode organization = manifest.DocumentElement;
            XmlNode item = organization.SelectSingleNode("//mn:item",nsmgr);
            string idref = item.Attributes["identifierref"].Value;
            XmlNode resource = organization.SelectSingleNode("//mn:resource[@identifier='" + idref + "']",nsmgr);
            var href = resource.Attributes["href"].Value;
            //string idref = (string)manifest.Root.Descendants("item").FirstOrDefault().Attribute("identifierref");
            //string href = (string)manifest.Root.Descendants("resource").FirstOrDefault(b => (string)b.Attribute("identifier") == idref).Attribute("href");
            return href;
        }
    }
}