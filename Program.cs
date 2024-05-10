using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//cookie here is an authentication schema
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie")
    .AddOAuth("github-oauth", o =>
    {
        o.ClientId = "Ov23liaLD8Q2gChv8a2j";
        o.ClientSecret = "6708205304c639fffea495cc7414a2117b9d9002";
        o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        o.TokenEndpoint = "https://github.com/login/oauth/access_token";

        o.UserInformationEndpoint = "https://api.github.com/user";
        o.CallbackPath = "/github-oauth-cb";

        o.SignInScheme = "cookie";
    });

var app = builder.Build();


app.MapGet("/username", (HttpContext ctx, IDataProtectionProvider idp) =>
{
    return "";
    //ctx.User.FindFirst("usr")?.Value
});

app.MapGet("/login", () =>
{
    return "Hello world!";
});

app.MapGet("/login-github", () =>
{
    return Results.Challenge(new AuthenticationProperties()
    {
        RedirectUri = "http://localhost:5005/username"

    }, authenticationSchemes: new List<string>() { "github-oauth" });
});

app.Run();


/*public class AuthService
{
    private readonly IDataProtectionProvider _idp;
    private readonly IHttpContextAccessor _accessor;

    public AuthService(IDataProtectionProvider idp, IHttpContextAccessor accessor)
    {
        _idp = idp;
        _accessor = accessor;
    }

    public void SignIn()
    {
        var protector = _idp.CreateProtector("auth-cookie");
        _accessor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:manu")}";
    }


}*/

