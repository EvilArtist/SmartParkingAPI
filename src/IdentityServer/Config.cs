// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using SmartParking.Share.Constants;
using IdentityModel;
using IdentityServer4;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(IdentityConstants.Scope.Api, new[] {
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Id
                })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(IdentityConstants.Scope.Scope1),
                new ApiScope(IdentityConstants.Scope.Scope2),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = IdentityConstants.ClientId.Api,
                    ClientName = "Client Password Claims",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(IdentityConstants.ClientIdSecret.Api.Sha256()) },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityConstants.Scope.Api
                    }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = IdentityConstants.ClientId.Interactive,
                    ClientSecrets = { new Secret(IdentityConstants.ClientIdSecret.Interactive.Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44300/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityConstants.Scope.Scope1
                    }
                },
                new Client
                {
                    ClientName = "Angular-Client",
                    ClientId = IdentityConstants.ClientId.Spa,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string>{ "http://localhost:4200/signin-callback", 
                        "http://localhost:4200/assets/silent-callback.html" },
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityConstants.Scope.Api
                    },
                    AllowedCorsOrigins = { "http://localhost:4200" },
                    RequirePkce = true,
                    RequireClientSecret = false,
                    PostLogoutRedirectUris = new List<string> { "http://localhost:4200/signout-callback" },
                    RequireConsent = false,
                    AccessTokenLifetime = 600
                }
            };
    }
}