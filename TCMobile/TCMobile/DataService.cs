using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
namespace TCMobile
{
    public class DataService
    {
        public static async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<Catalogue>(json);
            }

            return data;
        }

        public static async Task<dynamic> getLPs(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<LPS>(json);
            }

            return data;
        }

        public static async Task<dynamic> getLPDetails(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                JsonSerializerSettings ser = new JsonSerializerSettings();
                ser.DefaultValueHandling = DefaultValueHandling.Populate;
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<Map>(json,ser);
            }

            return data;
        }

        public static async Task<dynamic> contactLMS(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<LogInObj>(json);
            }

            return data;
        }

        public static async Task<dynamic> commitToLMS(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                try
                {
                    string xmlString = response.Content.ReadAsStringAsync().Result;
                    XmlDocument result = new XmlDocument();
                    var xdoc = XDocument.Parse(xmlString);
                    data = xdoc.Descendants("{http://www.thinkingcap.com/}Success").First().Value;
                    //data = JsonConvert.DeserializeObject<ServiceResultOfString>(result.SelectSingleNode(");
                    // data = result.SelectSingleNode("root/ServiceResultOfString/Success");
                }catch(System.Exception ex)
                {
                    Crashes.TrackError(ex);
                    data = "Error";
                }
            }

            return data;
        }

        public static async Task<string> GetCMI(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            string data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                try
                {
                    //data = JsonConvert.DeserializeObject<ServiceResultOfString>(json);
                    
                    var xdoc = XDocument.Parse(json);
                    data = xdoc.Descendants("{http://www.thinkingcap.com/}Result").First().Value;
                    //cmi = JsonConvert.DeserializeObject<API.Cmi>(data);
                }
                catch(System.Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                
            }

            return data;
        }

        public async Task LoginAsync(string username,  string password)
        {
            var keyvalues = new List<KeyValuePair<string ,string>>
            {
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", "password")

            };

            var request = new HttpRequestMessage(
                HttpMethod.Post, "https://tc.stable.thinkingcap.com/SAML/IdP/SSOService.aspx"
                );
            request.Content = new FormUrlEncodedContent(keyvalues);
            var client = new HttpClient();
            var response = await client.SendAsync(request);
        }
    }
}