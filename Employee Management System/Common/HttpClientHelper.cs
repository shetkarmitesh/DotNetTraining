using System.Text;

namespace Employee_Management_System.Common
{
    public class HttpClientHelper
    {
        //makegpostrequest

        public static async Task<string> MakePostRequest(string baseUrl, string endpoint, string
            apiRequestData)
        {
            var socketHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10,
            };

            using (HttpClient httpClient = new HttpClient(socketHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri(baseUrl);
                StringContent apiRequestContent = new StringContent(apiRequestData, Encoding.UTF8, "application/json");

                var httpResponse = httpClient.PostAsync(endpoint, apiRequestContent).Result;
                var httpResponseString = httpResponse.Content.ReadAsStringAsync().Result;

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponseString);
                }

                return httpResponseString;
            }

        }

        //makegetrequest

        public static async Task<string> MakeGetRequest(string baseUrl, string endpoint)
        {
            var socketHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10,
            };

            using (HttpClient httpClient = new HttpClient(socketHandler))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri(baseUrl);

                var httpResponse = await httpClient.GetAsync(endpoint);
                var httpResponseString = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponseString);
                }

                return httpResponseString;
            }
        }
    }
}
