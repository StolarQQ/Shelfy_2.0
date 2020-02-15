using Microsoft.Extensions.DependencyInjection;
using Shelfy.Core.Domain;

namespace Shelfy.API.Framework.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AuthorizationExtension(this IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy("HasUserRole", p => p.RequireRole(Role.User.ToString(), Role.Admin.ToString()));
                x.AddPolicy("HasModeratorRole", p => p.RequireRole(Role.Moderator.ToString(), Role.Admin.ToString()));
                x.AddPolicy("HasAdminRole", p => p.RequireRole(Role.Admin.ToString()));
            });

            return services;
        }
    }
}