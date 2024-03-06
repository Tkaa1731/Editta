using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Data.Tables.InteractionTables;
using Pobytne.Server;
using Pobytne.Server.Extensions;
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
				o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = new TimeSpan(0, 0, 30),
					//ValidIssuer = builder.Configuration["JWT:ValidIssuer"],// TODO: Vymyslet co s Audience a Issuer.. Potrebuju je?
					//ValidAudience = builder.Configuration["JWT:ValidAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!)),
				};
            });
            builder.Services.AddAuthorization();
			builder.Services.AddSingleton<IAuthorizationHandler, PermitionRequirementHandler>();
			builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<LicenseService>();
			builder.Services.AddScoped<ModuleService>();
			builder.Services.AddScoped<UserService>();
			builder.Services.AddScoped<PermitionService>();
            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<RecordService>();
            builder.Services.AddScoped<InteractionService>();
            builder.Services.AddScoped<PaymentService>();
            builder.Services.AddScoped<AuthService>();


            builder.Services.AddScoped<LicenseTable>();
            builder.Services.AddScoped<ModuleTable>();
			builder.Services.AddScoped<UserTable>();
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