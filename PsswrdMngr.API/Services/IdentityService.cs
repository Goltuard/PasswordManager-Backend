using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.API.Services;

public static class IdentityService
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<User>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<DataContext>();
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["DebugTokenKey"]));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        
        services.AddScoped<TokenService>();
        
        return services;
    }
}