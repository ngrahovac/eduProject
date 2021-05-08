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
using eduProjectModel.Input;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Tewr.Blazor.FileReader;

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

            builder.Services.AddSingleton<ProjectInputModel>();


            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            //TEST
            builder.Services.AddScoped<ApiAuthenticationStateProvider>();

            builder.Services.AddFileReaderService(o => o.UseWasmSharedBuffer = true);

            await builder.Build().RunAsync();
        }
    }
}
