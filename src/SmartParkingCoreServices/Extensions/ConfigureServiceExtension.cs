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
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using IdentityConstants = SmartParking.Share.Constants.IdentityConstants;
using IdentityServer4;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace SmartParkingCoreServices.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static void ConfigIdentityDbContext(this IServiceCollection services, IConfiguration configuration, string migrationsAssembly)
        {
            var connectionString = configuration.GetConnectionString("Identity");

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDbContext<ApplicationIdentityContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        public static void ConfigCustomizeService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, EmployeeManagerService>();
            services.AddScoped<IRandomGeneratorService, RandomGeneratorService>();
        }

        public static void ConfigIdnentityAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", IdentityConstants.Scope.Api,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile);
                });

                var claims = RoleClaims.GetClaims();
                claims.ForEach(claim =>
                {
                    options.AddPolicy(claim, policy => policy.RequireClaim(claim));
                });
            });
        }

        public static void ConfigMainAuthorization(this IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationUser>()
              .AddRoles<ApplicationRole>()
              .AddDefaultTokenProviders()
              .AddEntityFrameworkStores<ApplicationIdentityContext>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie("Cookies", options => { options.Cookie.SameSite = SameSiteMode.Lax; });

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
                    policy.RequireClaim("scope", IdentityConstants.Scope.Api,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile);
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
