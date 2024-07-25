using Shared.Domain;

namespace Presentation.Endpoints;


public static partial class Users
{
    public static WebApplication  MapUserEndpoints (this WebApplication app)    
    {
        app.MapPost(Login.Endpoint, Login.Handler).WithOpenApi();
        app.MapPost(Register.Endpoint, Register.Handler).WithOpenApi();
        app.MapPost(ForgotPassword.Endpoint, ForgotPassword.Handler).WithOpenApi();
        app.MapPost(ResetPassword.Endpoint, ResetPassword.Handler).WithOpenApi();
        app.MapPost(SendVerificationCode.Endpoint, SendVerificationCode.Handler).WithOpenApi();
        app.MapPost(VerifyCode.Endpoint, VerifyCode.Handler).WithOpenApi();
        
        app.MapPut(UpdateData.Endpoint, UpdateData.Handler).RequireAuthorization(AppPermissions.UserPolicy).WithOpenApi();
        app.MapPut(UpdatePassword.Endpoint, UpdatePassword.Handler).RequireAuthorization(AppPermissions.UserPolicy).WithOpenApi();
        app.MapPut(UpdateProfile.Endpoint, UpdateProfile.Handler).RequireAuthorization(AppPermissions.UserPolicy).WithOpenApi();
        app.MapPut(Suspend.Endpoint, Suspend.Handler).RequireAuthorization(AppPermissions.AdministratorPolicy).WithOpenApi();
        
        app.MapDelete(Suspend.Endpoint, Suspend.Handler).RequireAuthorization(AppPermissions.UserPolicy).WithOpenApi();
        return app;
    }
}