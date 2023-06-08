using BusinessLogic.Options;
using BusinessLogic.Services;
using BusinessLogic.Services.Abstractions;
using BusinessLogic.Services.Repositories;
using DataAccess.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddBearerAuthentication(this IServiceCollection services)
        {
            var jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            return services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });
        }

        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ISeeder, Seeder>()
                .AddTransient<IUserRepository, UserRepository>();
        }
    }
}
