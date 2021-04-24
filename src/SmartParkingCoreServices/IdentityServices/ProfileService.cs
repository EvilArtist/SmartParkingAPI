using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartParkingCoreModels.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingCoreServices.IdentityServices
{
    public class ProfileService : DefaultProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ProfileService(ILogger<ProfileService> logger, 
                UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager,
                IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory) : base(logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            
            var roles = await userManager.GetRolesAsync(user);
            var applicationRoles = await roleManager.Roles.Where(x => roles.Contains(x.Name)).ToListAsync();
            var claims = roles.Select(role => new Claim(JwtClaimTypes.Role, role));
            context.IssuedClaims.AddRange(claims);
            foreach (var role in applicationRoles)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                if (roleClaims != null)
                {
                    var issueRoleClaims = roleClaims.Select(claim => new Claim(claim.Type, claim.Value));
                    context.IssuedClaims.AddRange(issueRoleClaims);
                }
            }

            context.IssuedClaims.Add(new Claim(CustomClaimTypes.Id, user.Id.ToString()));
            context.IssuedClaims.Add(new Claim(CustomClaimTypes.ClientId, user.ClientId ?? "Espace"));
        }
    }
}
