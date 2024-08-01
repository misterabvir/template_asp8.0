using MailService.Application;
using MailService.Infrastructure;
using MailService.Presentation;

var builder = WebApplication.CreateBuilder();


builder.Configuration.AddJsonFile("Settings/secrets.json", optional: false, reloadOnChange: true);

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);


var app = builder.Build();

app
    .UseInfrastructure()
    .UsePresentation()
    .Run();  