using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Data.Tables.InteractionTables;
using Pobytne.Server;
using Pobytne.Server.Authentication;
using Pobytne.Server.Service;
using System.Text;

namespace Pobytne
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PobytneAPI", Version = "v1" });
            });

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtAuthenticationManager.JWT_SECURITY_KEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    NameClaimType = "JwtAuth",
                    ValidateLifetime = true,
                };
            });
            builder.Services.AddScoped<UserService>();
			builder.Services.AddScoped<ModuleService>();
            builder.Services.AddScoped<LicenseService>();
            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<RecordService>();
            builder.Services.AddScoped<InteractionService>();
            builder.Services.AddScoped<PaymentService>();


            builder.Services.AddScoped<UserTable>();
            builder.Services.AddScoped<LicenseTable>();
            builder.Services.AddScoped<ModuleTable>();
            builder.Services.AddScoped<PermitionTable>();
            builder.Services.AddScoped<ClientTable>();
            builder.Services.AddScoped<RecordTable>();
            builder.Services.AddScoped<InteractionTable>();
            builder.Services.AddScoped<EvidenceTable>();
            builder.Services.AddScoped<CashRegisterTable>();
            builder.Services.AddScoped<PaymentTable>();


            Database.OnInitialize();
            SimpleCRUD.SetColumnNameResolver(new MyFluentMapperNameResolver());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor api v1");
                    c.RoutePrefix = "swagger";
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}