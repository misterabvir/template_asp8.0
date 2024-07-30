using Application;
using Infrastructure;

using Presentation;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);


builder.Build().UsePresentation().Run();  