// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Identity;
using SmartParking.Share.Constants;

namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging()
                .AddDbContext<ApplicationIdentityContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)));

            services.AddOperationalDbContext(options =>
            {
                options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });
            services.AddConfigurationDbContext(options =>
            {
                options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext>()
                .AddDefaultTokenProviders();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            MigrateApplicationDb(scope);
            SeedRoles(scope);
            SeedUser(scope);

            SeedIdentityConfig(scope);
        }

        private static void MigrateApplicationDb(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<ApplicationIdentityContext>();
            context.Database.Migrate();
        }

        private static void SeedRoles(IServiceScope scope)
        {
            var roleManage = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            if (!roleManage.Roles.Any())
            {
                foreach (var role in Roles.DefaultRoles)
                {
                    var applicationRole = new ApplicationRole()
                    {
                        Name = role
                    };
                    var result = roleManage.CreateAsync(applicationRole).Result;
                    if (role == Roles.SuperAdmin)
                    {
                        var claims = RoleClaims.GetClaims();
                        claims.ForEach(claim =>
                        {
                            roleManage.AddClaimAsync(applicationRole, new Claim(claim, claim)).Wait();
                            Log.Debug($"Add Role Claim {claim}");
                        });
                    }
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug($"Role {role} created");
                }
            }
        }

        private static void SeedUser(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var espace = userMgr.FindByNameAsync("ESPACE").Result;

            if (espace == null)
            {
                espace = new ApplicationUser
                {
                    UserName = "ESPACE",
                    Email = "espaceadmin@espace.edu.vn",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(espace, "1234@Password").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(espace, new Claim[]{
                            new Claim(JwtClaimTypes.WebSite, "http://espace.edu.vn")
                        }).Result;

                userMgr.AddToRoleAsync(espace, Roles.SuperAdmin).Wait();
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("ESPACE created");
            }
            else
            {
                Log.Debug("ESPACE already exists");
            }
        }

        private static void SeedIdentityConfig(IServiceScope scope)
        {
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                    Log.Debug($"Client {client.ClientName} added");
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                    Log.Debug($"Client {resource.DisplayName} added");
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                    Log.Debug($"Client {resource.DisplayName} added");
                }
                context.SaveChanges();
            }
        }
    }
}
