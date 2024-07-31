using Domain.Abstractions;

namespace AuthenticationService.Presentation.Endpoints;


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
        app.MapPost(Restore.Endpoint, Restore.Handler).WithOpenApi();
        
        app.MapPut(UpdateUsername.Endpoint, UpdateUsername.Handler).RequireAuthorization(AuthorizationConstants.Policies.User).WithOpenApi();
        app.MapPut(UpdatePassword.Endpoint, UpdatePassword.Handler).RequireAuthorization(AuthorizationConstants.Policies.User).WithOpenApi();
        app.MapPut(UpdateProfile.Endpoint, UpdateProfile.Handler).RequireAuthorization(AuthorizationConstants.Policies.User).WithOpenApi();
        app.MapPut(Suspend.Endpoint, Suspend.Handler).RequireAuthorization(AuthorizationConstants.Policies.Administrator).WithOpenApi();
        
        app.MapDelete(Suspend.Endpoint, Suspend.Handler).RequireAuthorization(AuthorizationConstants.Policies.User).WithOpenApi();
        return app;
    }
}