using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

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
    }
}