
using IdentityServer4;
using SmartParking.Share.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace IdentityServer.Helpers
{
    public class SiteHelpers
    {
        public static string GetGrantTypeName(string grantType)
        {
            return grantType switch
            {
                GrantTypes.Password => "Password",
                GrantTypes.Implicit => "Implicit",
                GrantTypes.AuthorizationCode => "Authorization Code",
                _ => grantType,
            };
        }

        public static ChipViewModel[] GrantTypesChip =
        {
            new ChipViewModel()
            {
                Id = 0, Code = GrantTypes.Password, DisplayName= "Password"
            },
            new ChipViewModel()
            {
                Id = 1, Code = GrantTypes.Implicit, DisplayName= "Implicit"
            },
            new ChipViewModel()
            {
                Id = 2, Code = GrantTypes.AuthorizationCode, DisplayName= "Authorization Code"
            },
            new ChipViewModel()
            {
                Id = 3, Code = GrantTypes.ClientCredentials, DisplayName= "Client Credentials"
            }
        };

        public static ChipViewModel[] ScopesChip =
        {
            new ChipViewModel()
            {
                Id = 0, Code = IdentityConstants.Scope.Api, DisplayName= "API", Selected = true
            },
            new ChipViewModel()
            {
                Id = 1, Code = IdentityServerConstants.StandardScopes.OpenId, DisplayName= "OpenId", Selected = true
            },
            new ChipViewModel()
            {
                Id = 2, Code = IdentityServerConstants.StandardScopes.Profile, DisplayName= "Profile", Selected = true
            },
            new ChipViewModel()
            {
                Id = 3, Code = "custom", DisplayName= "Custom", Selected = false
            }
        };

        public static string GetScopeName(string scope)
        {
            return scope;
        }
    }

    public class ChipViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public bool Selected { get; set; }
    }
}
