using Microsoft.Extensions.DependencyInjection;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Admin;
using SmartParkingAbstract.Services.General;
using SmartParkingCoreServices.Admin;
using SmartParkingCoreServices.General;
using Microsoft.IdentityModel.Tokens;
using SmartParkingCoreModels.Identity;
using SmartParkingCoreModels.Data;
using Microsoft.AspNetCore.Identity;

namespace SmartParkingCoreServices.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static void ConfigCustomizeService(this IServiceCollection service)
        {
            service.AddScoped<IUserService, EmployeeManagerService>();
            service.AddScoped<IRandomGeneratorService, RandomGeneratorService>();
        }

        public static void ConfigIdnentityAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(option =>
            {
                var claims = RoleClaims.GetClaims();
                claims.ForEach(claim =>
                {
                    option.AddPolicy(claim, policy => policy.RequireClaim(claim));
                });
            });
        }

        public static void ConfigMainAuthorization(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api");
                });

                var claims = RoleClaims.GetClaims();
                claims.ForEach(claim =>
                {
                    options.AddPolicy(claim, policy => policy.RequireClaim(claim));
                });
            });
        }
    }
}
