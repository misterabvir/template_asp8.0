using MailService.Application;
using MailService.Infrastructure;
using MailService.Presentation;

var builder = WebApplication.CreateBuilder();

builder.Configuration.AddJsonFile("Common/Settings/token-settings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("Common/Settings/secrets.json", optional: false, reloadOnChange: true);

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);


builder.Build().UsePresentation().Run();  