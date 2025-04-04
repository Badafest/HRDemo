﻿using System.Net.Http.Headers;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace HRDemoAdmin.Services
{
    public interface IApiResponse
    {
        bool Success { get; set; }
        JObject ErrorResponse { get; set; }
    }
    public class ApiResponse<T> : IApiResponse
    {
        public T Data { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public bool Success { get; set; }
        public JObject ErrorResponse { get; set; }
    }
    public class ServiceBase
    {
        private readonly string _baseUrl;
        private readonly string _bearerToken;

        public ServiceBase(string baseUrl, string bearerToken = null)
        {
            _baseUrl = baseUrl;
            _bearerToken = bearerToken;
        }

        public ApiResponse<T> Get<T>(string endpoint, object queryObject = null)
        {
            var queryData = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(queryObject));
            if (queryData != null)
            {
                endpoint += "?";
                int index = 1;
                foreach (var keyValuePair in queryData)
                {
                    endpoint += $"{keyValuePair.Key}={keyValuePair.Value ?? ""}" + (index == queryData.Count ? "" : "&");
                    index++;
                }
            }
            return CallApi<T>(endpoint, HttpMethod.Get);
        }

        public ApiResponse<T> Delete<T>(string endpoint)
        {
            return CallApi<T>(endpoint, HttpMethod.Delete);
        }

        public ApiResponse<T> Post<T>(string endpoint, object content = null)
        {
            return CallApi<T>(endpoint, HttpMethod.Post, content);
        }

        public ApiResponse<T> Put<T>(string endpoint, object content = null)
        {
            return CallApi<T>(endpoint, HttpMethod.Put, content);
        }
        public ApiResponse<T> Patch<T>(string endpoint, object content = null)
        {
            return CallApi<T>(endpoint, new HttpMethod("PATCH"), content);
        }
        private ApiResponse<T> CallApi<T>(string endpoint, HttpMethod method, object content = null)
        {
            using (HttpClientHandler handler = new HttpClientHandler { 
                UseDefaultCredentials = string.IsNullOrEmpty(_bearerToken) 
            }) // Pass Windows credentials

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (!string.IsNullOrEmpty(_bearerToken))
                {
                    client.DefaultRequestHeaders.Add("Authorization", _bearerToken);
                }
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(_baseUrl + endpoint),
                    Method = method,
                    Content = content == null ? null : new StringContent(
                        JsonConvert.SerializeObject(content),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json"),
                };
                HttpResponseMessage response = Task.Run(() => client.SendAsync(request)).Result;
                string json = Task.Run(() => response.Content.ReadAsStringAsync()).Result;

                var headersDictionary = new Dictionary<string, string>();
                foreach (var header in response.Headers)
                {
                    headersDictionary.Add(header.Key, header.Value.FirstOrDefault());
                }
                var apiResponse = new ApiResponse<T>
                {
                    Success = response.IsSuccessStatusCode,
                    Headers = headersDictionary
                };
                if (apiResponse.Success)
                {
                    apiResponse.Data = JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    apiResponse.ErrorResponse = JsonConvert.DeserializeObject<JObject>(json) ?? new JObject();
                    apiResponse.ErrorResponse.Add("StatusCode", JToken.FromObject(response.StatusCode));
                }
                return apiResponse;
            }
        }
    }
}
