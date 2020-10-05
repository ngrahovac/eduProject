using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Services
{
    public class ApiService
    {
        private readonly HttpClient httpClient;

        public ApiService()
        {
        }

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            return await httpClient.GetFromJsonAsync<T>(url);
        }
    }
}

