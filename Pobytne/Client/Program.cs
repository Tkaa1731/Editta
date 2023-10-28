using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Pobytne.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Extensions;
using Pobytne.Shared.Procedural;
using Microsoft.AspNetCore.Authorization;

namespace Pobytne.Client
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // ...
            services.AddBlazoredLocalStorage();
            services.AddHxServices();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddAuthorizationCore();
            services.AddScoped<IAuthorizationHandler, PermitionRequirementHandler>();
            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy("PermitionPolicy", policy => policy.AddRequirements(new PermitionRequirement()));
            });
            // ...
        }
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }
    }
}