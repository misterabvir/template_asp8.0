using Domain.Persistence;
using Infrastructure;
using Application;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// TODO : Replace this on user secrets
builder.Configuration.AddJsonFile("Settings/appsettings.Secrets.json", optional: false, reloadOnChange: true);

// configuration
builder.Configuration.AddJsonFile("Settings/cleaner-settings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("Settings/encrypt-settings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("Settings/outbox-settings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("Settings/token-settings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("Settings/verification-settings.json", optional: false, reloadOnChange: true);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddPresentation(builder.Configuration);

builder.Build().UsePresentation().Run();

