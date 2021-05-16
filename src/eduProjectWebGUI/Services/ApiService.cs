using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace eduProjectWebGUI.Services
{
    public class ApiService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public ApiService()
        {

        }

        public ApiService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T obj)
        {
            var token = await localStorage.GetItemAsStringAsync("authToken");
            var authHeaderValue = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = authHeaderValue;
            request.Content = JsonContent.Create<T>(obj);

            var result = await httpClient.SendAsync(request);
            return result;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T obj)
        {
            var token = await localStorage.GetItemAsStringAsync("authToken");
            var authHeaderValue = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = authHeaderValue;
            request.Content = JsonContent.Create<T>(obj);

            var result = await httpClient.SendAsync(request);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var token = await localStorage.GetItemAsStringAsync("authToken");
            var authHeaderValue = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = authHeaderValue;

            var result = await httpClient.SendAsync(request);
            return result;
        }

        public async Task<T> GetAsync<T>(string url) //second arg: AuthenticationHeaderValue authorization
        {
            var token = await localStorage.GetItemAsStringAsync("authToken");
            var authHeaderValue = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = authHeaderValue;

            var responseMessage = await httpClient.SendAsync(request);
            var result = await responseMessage.Content.ReadAsStringAsync();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(result);
            writer.Flush();
            stream.Position = 0;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }
    }
}

