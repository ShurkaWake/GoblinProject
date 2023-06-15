using AutoFilterer.Swagger;
using BusinessLogic.Abstractions;
using BusinessLogic.Options;
using BusinessLogic.Services;
using BusinessLogic.Services.Repositories;
using DataAccess.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
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
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IBusinessRepository, BusinessRepository>()
                .AddTransient<IScalesRepository, ScalesRepository>()
                .AddTransient<IBusinessService, BusinessService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<IExchangeService, ExchangeService>()
                .AddTransient<IHashService, HashService>()
                .AddTransient<IMeasurementService, MeasurementService>()
                .AddTransient<IResourceService, ResourceService>()
                .AddTransient<IScalesService, ScalesService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IWorkingShiftService, WorkingShiftService>()
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IStatisticService, StatisticService>()
                ;
        }

        public static IServiceCollection AddServicesOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<SeederOptions>(
                    configuration.GetSection(SeederOptions.Section))
                .Configure<JwtOptions>(
                    configuration.GetSection(JwtOptions.Section))
                .Configure<EmailOptions>(
                    configuration.GetSection(EmailOptions.Section));
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GenericWebApi", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Authorization using Bearer scheme 'Bearer <token>'",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.UseAutoFiltererParameters();
            });

            return services;
        }
    }
}
