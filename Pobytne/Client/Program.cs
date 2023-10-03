using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Pobytne.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Extensions;
using Pobytne.Shared.Procedural;

namespace Pobytne.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthenticationStateProvider>();


            builder.Services.AddHxServices();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}