using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Pages;
using MailKit.Net.Smtp;
using MimeKit;

namespace Pobytne.Server.Service
{
    public class MailService(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider ,IConfiguration configuration)
    {
        private readonly IRazorViewEngine _viewEngine = viewEngine;
        private readonly ITempDataProvider _tempDataProvider = tempDataProvider;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly IConfiguration _configuration = configuration;
        private IConfigurationSection EmailConfig => _configuration.GetSection("EmailSettings");

        public async Task SendRestorePasswordMail(ResetPasswordEmailModel model)
        {
            var jwtSection = _configuration.GetSection("JWT");
            model.Uri = jwtSection["ValidIssuer"]!; 

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(EmailConfig["FromName"], EmailConfig["FromEmail"]));
            mimeMessage.To.Add(MailboxAddress.Parse(model.Email));
            mimeMessage.Subject = "EDITTA | Nastavení hesla";

            var builder = new BodyBuilder();
            builder.HtmlBody = await RenderViewToStringAsync("Pages/ResetPasswordEmail.cshtml",model); 

            mimeMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(EmailConfig["SmtpServer"], int.Parse(EmailConfig["SmtpPort"]!));
            await client.AuthenticateAsync(EmailConfig["SmtpUserName"], EmailConfig["SmtpPassword"]);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
        private async Task<string> RenderViewToStringAsync(string viewPath, ResetPasswordEmailModel model)
        {
            DefaultHttpContext defaultHttpContext = new()
            {
                RequestServices = _serviceProvider
            };
            ActionContext actionContext = new(defaultHttpContext, new RouteData(), new ActionDescriptor());

            IView? view;
            ViewEngineResult viewEngineResult = _viewEngine.GetView(null, viewPath, true);
            if (viewEngineResult.Success)
                view = viewEngineResult.View;
            else
                throw new InvalidOperationException($"Unable to find View {viewPath}.");

            await using StringWriter stringWriter = new();
            ViewDataDictionary<ResetPasswordEmailModel> viewDataDictionary = new(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model,
            };

            TempDataDictionary tempDataDictionary = new(actionContext.HttpContext, _tempDataProvider);
            ViewContext viewContext = new(
                actionContext,
                view,
                viewDataDictionary,
                tempDataDictionary,
                stringWriter,
                new HtmlHelperOptions()
            );
            await view.RenderAsync(viewContext);

            return stringWriter.ToString();
        }
    }
}
