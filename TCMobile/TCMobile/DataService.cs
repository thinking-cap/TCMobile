using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;

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
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<ServiceResultOfString>(json);
            }

            return data;
        }

        public static async Task<dynamic> GetCMI(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<ServiceResultOfString>(json);
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