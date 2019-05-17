using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Shelfy.API.Framework.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection RegisterJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    //cfg.RequireHttpsMetadata = false;
                    //cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        // validate recipient of the token, 
                        ValidateAudience = false,
                        //  verify that the key used to sign the incoming token is part of a list of trusted keys 
                        ValidateIssuerSigningKey = true,
                        // creator of Token
                        ValidIssuer = configuration["Jwt:issuer"],
                        ValidateLifetime = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
                    };
                });

            return services;
        }
    }
}
