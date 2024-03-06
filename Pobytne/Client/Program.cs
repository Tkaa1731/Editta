using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Pobytne.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Authorization;
using Pobytne.Client.Services;
using Microsoft.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Pobytne.Client.Extensions;

namespace Pobytne.Client
{
    public class Program
    {
        public static void ConfigureServices(IServiceCollection services)
		{
			// ...
			services.AddCascadingValue(sp =>
			{
				var workplace = new ModuleWorkplace { Id = -1};
				var source = new CascadingValueSource<ModuleWorkplace>(workplace, isFixed: false);
				return source;
			});
			services.AddBlazoredLocalStorage();
            services.AddHxServices();
            services.AddHxMessenger();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddScoped<ExcelExportService>();
			services.AddScoped<IAuthorizationHandler, PermitionRequirementHandler>();
            services.AddAuthorizationCore(config =>
            {
                config.AddPolicy("PermitionPolicy", policy => policy.AddRequirements(new PermitionRequirement()));
            });
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
            });

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }
    }
}