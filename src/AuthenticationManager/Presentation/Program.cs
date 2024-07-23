using Infrastructure;
using Persistence;
using Application;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddPresentation(builder.Configuration);

builder.Build().UsePresentation().Run();
