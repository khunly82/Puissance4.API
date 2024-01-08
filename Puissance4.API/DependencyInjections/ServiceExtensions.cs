using Be.Khunly.Security;
using System.IdentityModel.Tokens.Jwt;

namespace Puissance4.API.DependencyInjections
{
    public static class ServiceExtensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration config)
        {
            JwtManager.JwtConfig? jconfig = config.GetSection("Jwt").Get<JwtManager.JwtConfig>();
            if (jconfig is null)
            {
                throw new Exception("jwt config is missing");
            }

            services.AddSingleton(jconfig);
            services.AddScoped<JwtSecurityTokenHandler>();
            services.AddScoped<JwtManager>();
        }
    }
}
