using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.Modal;
using eduProjectWebGUI.Services;

namespace eduProjectWebGUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped<ApiService>();

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(@"https://localhost:44345/") });

            builder.Services.AddBlazoredModal();

            builder.Services.AddSingleton<ProjectApplicationInputModel>();

            await builder.Build().RunAsync();
        }
    }
}
