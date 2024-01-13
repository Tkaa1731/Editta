using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Pobytne.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Authorization;
using Pobytne.Client.Services;

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

			services.AddScoped<IAuthorizationHandler, PermitionRequirementHandler>();
            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy("PermitionPolicy", policy => policy.AddRequirements(new PermitionRequirement()));
            });
            services.AddTransient<TokenService>();
            services.AddTransient<JwtTokenInterceptor>();
            services.AddScoped<AuthenticationService>();
            // ...
        }
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient<PobytneService>(client =>
            {    // Konfigurace HTTP klienta
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            }).AddHttpMessageHandler<JwtTokenInterceptor>();

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }
    }
}