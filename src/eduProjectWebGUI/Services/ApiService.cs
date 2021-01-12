using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Services
{
    public class ApiService
    {
        private readonly HttpClient httpClient;

        public ApiService() { }

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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

        //__________________________________________________

        public async Task<string> GetJsonAsync<T>(string url, AuthenticationHeaderValue authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = authorization;

            var responseMessage = await httpClient.SendAsync(request);
            
            var result = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine(result); //

            return result;

            /*
             - od http response poruke CONTENT ima metodu za pretvaranje u string tog rezultata
            - JsonSerializer ima metodu JsonSerializer.Deserialize<WeatherForecastWithPOCOs>(jsonString) koja od stringa pravi objekat
             */
        }

    }
}

