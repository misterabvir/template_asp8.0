using AuthenticationService.Presentation.Endpoints;

namespace AuthenticationService.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();    

        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.MapUserEndpoints();
        return app;
    }
}