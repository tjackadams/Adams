using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorStrap;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using WebBlazor.Client.Infrastructure.HttpClients;

namespace WebBlazor.Client
{
    public class Program
    { 
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            builder.Services.AddHttpClient(HttpClients.SmokingClient,
                    client => { client.BaseAddress = new Uri(builder.Configuration["SmokingUrl"]); })
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
                .AddPolicyHandler(GetRetryPolicy());

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            builder.Services.AddBootstrapCss();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            await builder.Build().RunAsync();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}