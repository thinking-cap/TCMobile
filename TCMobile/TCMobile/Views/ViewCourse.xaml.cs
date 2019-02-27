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
using Newtonsoft.Json;
using System.Dynamic;

namespace TCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewCourse : ContentPage
	{
        public static HybridWebView courseWindow;
       

        public class message
        {
            public string status { get; set; }
            public string cmi { get; set; }
        }     


        public ViewCourse (string courseid)
		{
            InitializeComponent();
            launchCourse(courseid);

        }

        private async void launchCourse(string courseid)
        {
            // create an api object
            API api = new API();
            // use MessagingCenter to talk to the webview //
            MessagingCenter.Subscribe<string>(this, "API", (cmi) =>
            {
                message APIMessage = JsonConvert.DeserializeObject<message>(cmi);
               
                string status = APIMessage.status;
                string CMIString = APIMessage.cmi;
                // if it's a commit then save the cmi object to the course record
                if (status == "Commit")
                {
                    api.Commit(CMIString, courseid);
                }else if(status == "Terminate")
                {
                    // api.CommitToLMS(CMIString, courseid); // not working yet
                    Navigation.PopAsync();
                }
            });

            // find the html path
            string launch = itemPath(courseid);
            string item_id = itemID(courseid);
            // get the cmi object

            string CMI = await cmiInit(courseid);
            // build the path to the local file
            string courseindex = "Courses/" + courseid + "/" + launch;
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string coursePath = Path.Combine(localFolder, courseindex);
            // get the api connector
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ViewCourse)).Assembly;
            string APIJS = "";
            using (var APIStream = assembly.GetManifestResourceStream("TCMobile.API.js"))
            {
                StreamReader apiReader = new StreamReader(APIStream);
                APIJS = apiReader.ReadToEnd();
            }
            // get the baseurl 
            String baseUrl = "file:/" + Path.GetDirectoryName(coursePath);
            FileStream stream = File.OpenRead(coursePath);
            if (stream == null)
            {
                throw new InvalidOperationException(
                    String.Format("Cannot create stream from specified URL: {0}", "course.htm"));
            }

           
            StreamReader reader = new StreamReader(stream);

          
            string htmlString = reader.ReadToEnd();

            courseWindow = new HybridWebView
            {                
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "TCMobile_CourseView"
            };


            // add the webview
           
            // create the webview
            HtmlWebViewSource html = new HtmlWebViewSource();

            if (!String.IsNullOrEmpty(CMI))
            {
                courseWindow.APIJS = APIJS + " var cmi=" + CMI;
            }
            else
            {
                API.Cmi cmi = new API.Cmi();
                cmi.course_id = courseid;
                cmi.sco_id = item_id;
                cmi.session_guid = new Guid().ToString();
                cmi.entry = "normal";
                cmi.learner_id = Constants.StudentID;
                cmi.learner_name = Constants.firstName + " " + Constants.lastName;
                cmi.score = new API.Score();
                cmi.comments = "";
                cmi.completion_status = "unknown";
                cmi.success_status = "unknown";
                cmi.suspend_data = "";
                cmi.session_time = "";
                cmi.total_time = "";
                cmi.score.scaled = "";
                cmi.score.raw = "";
                cmi.location = "";
                cmi.exit = "";
                cmi.objectives = new List<object>();
                cmi.interactions_data = new API.Interactions_Data();
                cmi.objectives_data = new API.Objectives_Data();
                cmi.objectives_data.objectives = new List<API.Objective>();
                cmi.interactions_data.interactions = new List<API.Interactions>();
                cmi.interactions_data._children = "id,type,objectives,timestamp,correct_responses,weighting,learner_response,result,latency,description";
                cmi.objectives_data._children = "id,score,success_status,completion_status,description";


                cmi.comments_from_learner = new API.CommentsFromLearner();
                cmi.interactions = new List<API.Interactions>();
                cmi.comments_from_learner.comments = new List<object>();
                cmi.comments_from_learner._children = "comment,location,timestamp";
                string cmiString = JsonConvert.SerializeObject(cmi);
                courseWindow.APIJS = APIJS + " var cmi=" + cmiString;
            }

            // set the base url
            html.BaseUrl = baseUrl;
            // pass in the API connector //
           
            // pass in the html
            courseWindow.Source = htmlString;
            // pass in the file Android
            courseWindow.Uri = coursePath;
            // pass in the file iOS
            courseWindow.iOSPath = courseindex;
            WebViewContainer.Children.Add(courseWindow);

        }

        private async Task<string>cmiInit(string courseid)
        {
            API a = new API();
            return await a.InitializeCourse(courseid);
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
     
        // get the item from the manifest.xml
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

            return href;
        }

        public string itemID(string courseid)
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
            XmlNode item = organization.SelectSingleNode("//mn:item", nsmgr);
            string id = item.Attributes["identifier"].Value;           

            return id;
        }
    }
}