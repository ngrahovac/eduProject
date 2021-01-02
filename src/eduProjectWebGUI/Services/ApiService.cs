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

        public async Task<T> GetAsync<T>(string url)
        {
            return await httpClient.GetFromJsonAsync<T>(url);
        }

        public async Task PostAsync<T>(string url, T obj)
        {
            await httpClient.PostAsJsonAsync(url, obj);
        }

        public async Task PutAsync<T>(string url, T obj)
        {
            await httpClient.PutAsJsonAsync(url, obj);
        }

        public async Task DeleteAsync(string url)
        {
            await httpClient.DeleteAsync(url);
        }

        public async Task<T> GetJsonAsync<T>(string url) //second arg: AuthenticationHeaderValue authorization
        {
            var token = await localStorage.GetItemAsStringAsync("authToken");
            var authHeaderValue = new AuthenticationHeaderValue("Bearer", token);

            //For testing
            if (localStorage == null)
                Console.WriteLine("ApiService: Local Storage is null");
            else
                Console.WriteLine("ApiService: Local Storage is not null");

            Console.WriteLine("ApiService: TOKEN=" + token);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = authHeaderValue;
            //request.Headers.Authorization = authorization;

            var responseMessage = await httpClient.SendAsync(request);
            var result = await responseMessage.Content.ReadAsStringAsync();

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(result);
            writer.Flush();
            stream.Position = 0;

            //For testing
            Console.WriteLine("Result from API: " + result);

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }

    }
}

