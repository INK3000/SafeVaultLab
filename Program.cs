using Microsoft.AspNetCore.Authentication.Cookies;
using SafeVault.Data;
using SafeVault.Security;
using SafeVault.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddSingleton<AuthService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok("SafeVault is running."));
app.MapGet("/access-denied", () => Results.StatusCode(StatusCodes.Status403Forbidden));

app.Run();
