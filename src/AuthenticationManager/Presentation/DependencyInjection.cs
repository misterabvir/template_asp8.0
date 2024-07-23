namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {


        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        return app;
    }
}