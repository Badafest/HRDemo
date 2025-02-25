using System.Net.Http.Headers;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HRDemoAdmin.Services
{
    public class ServiceBase
    {
        private readonly string _baseUrl;

        public ServiceBase(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public T Get<T>(string endpoint)
        {
            T result = CallApi<T>(endpoint, HttpMethod.Get);
            return result;
        }

        public T Delete<T>(string endpoint)
        {
            T result = CallApi<T>(endpoint, HttpMethod.Delete);
            return result;
        }

        public T Post<T>(string endpoint, object content)
        {
            T result = CallApi<T>(endpoint, HttpMethod.Post, content);
            return result;
        }

        public T Put<T>(string endpoint, object content)
        {
            T result = CallApi<T>(endpoint, HttpMethod.Put, content);
            return result;
        }
        public T Patch<T>(string endpoint, object content)
        {
            T result = CallApi<T>(endpoint, new HttpMethod("PATCH"), content);
            return result;
        }
        private T CallApi<T>(string endpoint, HttpMethod method, object content = null)
        {
            using (HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true }) // Pass Windows credentials
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_baseUrl + endpoint),
                    Method = method,
                    Content = content == null ? null : new StringContent(JsonConvert.SerializeObject(content))
                };
                HttpResponseMessage response =  Task.Run(() => client.SendAsync(request)).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new Exception("Failed to  receive response from API: " + response.StatusCode);
            }
        }
    }
}
