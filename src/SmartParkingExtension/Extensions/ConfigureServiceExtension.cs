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
using SmartParkingAbstract.Services.Parking;
using SmartParkingCoreServices.Parking;
using AutoMapper;
using SmartParkingCoreServices.AutoMap;
using SmartParkingAbstract.Services.Parking.PriceBook;
using SmartParkingCoreServices.Parking.PriceBook;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using SmartParkingAbstract.Services.Operation;
using SmartParkingCoreServices.Operation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System.IO;
using SmartParkingAbstract.Services.File;
using SmartParkingCoreServices.File;
using SmartParkingAbstract.Services.Data;
using SmartParkingExcel;
using System.Collections.Generic;

namespace SmartParkingExtensions
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

        public static void ConfigParkingDbContext(this IServiceCollection services, IConfiguration configuration, string migrationsAssembly)
        {
            var connectionString = configuration.GetConnectionString("SmartParking");

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        public static void ConfigCustomizeService(this IServiceCollection services)
        {
            services.ConfigAutomapper();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserService, EmployeeManagerService>();
            services.AddScoped<IRandomGeneratorService, RandomGeneratorService>();
            services.AddScoped<ISlotTypeService, SlotTypeService>();
            services.AddScoped<ISerialPortService, SerialPortService>();
            services.AddScoped<ICameraConfigService, CameraService>();
            services.AddScoped<IParkingService, ParkingService>();
            services.AddScoped<IParkingLaneService, ParkingLaneService>();
            services.AddScoped<ISlotTypeConfigurationService, SlotTypeConfigurationService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<ISubscriptionTypeService, SubscriptionTypeService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IHelpers, Helpers>();
            services.AddScoped<IPriceBookService, PriceBookService>();

            services.AddScoped<IPriceCalculationService, PriceCalculationService>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IFileService, FileService>();
        }

        public static void ConfigSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
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
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/operation")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
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

            //services.TryAddEnumerable(
            //ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
            //    ConfigureJwtBearerOptions>());
        }

        public static void ConfigAutomapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigFileServer(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
                RequestPath = new PathString("/Images")
            });
        }

        public static void ConfigDataParsingService(this IServiceCollection services)
        {
            services.AddTransient<ExcelDataParsingService>();

            services.AddTransient<DataParsingResolver>(serviceProvider => key =>
            {
                return key switch
                {
                    "EXCEL" => serviceProvider.GetService<ExcelDataParsingService>(),
                    _ => throw new KeyNotFoundException(),// or maybe return null, up to you
                };
            });
        }
    }
}
