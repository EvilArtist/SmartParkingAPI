using Microsoft.EntityFrameworkCore;
using SmartParkingAbstract.Services.Admin;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Users;
using SmartParkingCoreModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartParking.Share.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SmartParkingCoreModels.Identity;
using SmartParking.Share;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.General;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace SmartParkingCoreServices.Admin
{
    [Authorize(Policy = RoleClaims.EmployeeView)]
    public class EmployeeManagerService : IUserService
    {
        private readonly ApplicationIdentityContext dataContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IRandomGeneratorService randomGeneratorService;

        public EmployeeManagerService(ApplicationIdentityContext dataContext,
            IRandomGeneratorService randomGeneratorService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.dataContext = dataContext;
            this.randomGeneratorService = randomGeneratorService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<IdentityResult> CreateAdminAsync(AdminCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Phone))
            {
                throw new InvalidOperationException("Require_Email_OR_Phone");
            }
            string userName = string.IsNullOrEmpty(model.Email) ?
                model.Phone :
                model.Email.Remove(model.Email.IndexOf("@"));

            string password = randomGeneratorService.RandomPasswordString(12);
            Log.Debug($"Password Generated: '{password}'");
            ApplicationUser user = new()
            {
                UserName = userName,
                Email = model.Email,
                Address = model.Address,
                ClientId = model.ClientId,
                FirstName = model.FirstName,
                IDCardNumber = model.IDCardNumber,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Status = UserStatus.Active
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                result = await userManager.AddToRoleAsync(user, Roles.Admin);
            }
            return result;
        }

        public async Task<IdentityResult> CreateEmployeeAsync(EmployeeCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Phone))
            {
                throw new InvalidOperationException("Require_Email_OR_Phone");
            }
            //string userName = string.IsNullOrEmpty(model.Email) ?
            //    model.Phone :
            //    model.Email.Remove(model.Email.IndexOf("@"));

            string password = randomGeneratorService.RandomPasswordString(12);
            Log.Debug($"Password Generated: '{password}'");
            ApplicationUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                Address = model.Address,
                ClientId = model.ClientId,
                FirstName = model.FirstName,
                IDCardNumber = model.IDCardNumber,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Status = UserStatus.Active,
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var role = await dataContext.Roles.FindAsync(model.RoleId);

                result = await userManager.AddToRoleAsync(user, role.Name);
            }
            return result; ;
        }

        public async Task<IdentityResult> UpdateEmployeeAsync(Guid userId, EmployeeUpdateModel model)
        {
            ApplicationUser user = await dataContext.Users.FindAsync(userId);
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.IDCardNumber = model.IDCardNumber;
            var result = await userManager.UpdateAsync(user);
            var role = await roleManager.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleId);
            var currentRoles = await userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(role.Name))
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
                result = await userManager.AddToRoleAsync(user, role.Name);
            }
            return result;
        }

        public async Task<QueryResultModel<EmployeeViewModel>> GetManagedUserAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return QueryResultModel<EmployeeViewModel>.Empty();
            }
            var users = await dataContext.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role) 
                .Where(x=>x.ClientId == clientId)
                .PagedBy(1, 10)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    DisplayName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Phone = x.PhoneNumber,
                    Status = x.Status.ToString(),
                    Roles = x.UserRoles.Select(y=> new RoleViewModel
                    {
                        Id = y.RoleId,
                        DisplayName = y.Role.Name
                    }).ToList()
                }).ToListAsync();
            return new QueryResultModel<EmployeeViewModel>(users);
        }

        public async Task<QueryResultModel<EmployeeViewModel>> SearchManagedUserAsync(EmployeeQueryModel queryModels)
        {
            if(string.IsNullOrEmpty(queryModels.ClientId))
            {
                return QueryResultModel<EmployeeViewModel>.Empty();
            }
            var query = dataContext.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Where(x => x.ClientId == queryModels.ClientId);
            if (!string.IsNullOrEmpty(queryModels.QueryString))
            {
                query = query
                    .Where(x => x.UserName.ToUpper().Contains(queryModels.QueryString.ToUpper()) ||
                        x.Email.ToUpper().Contains(queryModels.QueryString.ToUpper()));
            }
            var users = await query
                .PagedBy(queryModels.Page, queryModels.PageSize)
                .Select(x => new EmployeeViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    DisplayName = x.FirstName + " " + x.LastName,
                    Email = x.Email,
                    Phone = x.PhoneNumber,
                    Status = x.Status.ToString(),
                    Roles = x.UserRoles.Select(y => new RoleViewModel
                    {
                        Id = y.RoleId,
                        DisplayName = y.Role.Name
                    }).ToList()
                }).ToListAsync();
            if (!string.IsNullOrEmpty(queryModels.Role))
            {
                users = users.Where(x => x.Roles.Any(y => y.DisplayName == queryModels.Role)).ToList();
            }
            return new QueryResultModel<EmployeeViewModel>(users);
        }

        public async Task<IEnumerable<RoleViewModel>> GetRolesAsync(string clientId)
        {
            var query = dataContext.Roles
                .Where(x => x.ClientId == clientId || string.IsNullOrEmpty(x.ClientId))
                .Where(x => !x.IsCustomerRole && x.Name != Roles.SuperAdmin)
                .Select(x => new RoleViewModel
                {
                    Id = x.Id,
                    DisplayName = x.Name
                });
            return await query.ToListAsync();
        }

        public async Task<EmployeeDetail> GetEmployeeById(Guid userId)
        {
            var query = dataContext.Users
                .Where(x => x.Id == userId)
                .Select( x => new EmployeeDetail
                {
                    Id = x.Id,
                    Address = x.Address,
                    Email = x.Email,
                    Phone = x.PhoneNumber,
                    FirstName = x.FirstName,
                    LastName = x.LastName, 
                    IDCardNumber = x.IDCardNumber,
                    RoleId = x.UserRoles.FirstOrDefault().RoleId,
                    Status = x.Status.ToString()
                })
                .FirstOrDefaultAsync();
             
            return await query;
        }

        public async Task<IEnumerable<PoliciesViewModel>> GetPoliciesAsync(string clientId)
        {
            var filterdRoles = dataContext.Roles.Where(x => x.ClientId == clientId || string.IsNullOrEmpty(x.ClientId));
            var roleClaims = await dataContext.RoleClaims.Join(filterdRoles, 
                claim => claim.RoleId, 
                role => role.Id, 
                (claim, role) => new
            {
                RoleName= role.Name,
                RoleId = role.Id,
                Claim = claim.ClaimType
            }).ToListAsync();
            var policies = roleClaims.GroupBy(x => x.RoleId, x=>x)
                .Select(rolePolicies => new PoliciesViewModel
                {
                    RoleId = rolePolicies.Key,
                    RoleName = rolePolicies.First().RoleName,
                    Policies = rolePolicies.Select(x => x.Claim)
                }).ToList();
            policies.Insert(0, new PoliciesViewModel()
            {
                RoleId = Guid.Empty,
                RoleName = "",
                Policies = RoleClaims.GetClaims()
            }) ;
            return policies;
        }


        public async Task<IEnumerable<PoliciesViewModel>> CreateRole(string RoleName)
        {
            var filterdRoles = dataContext.Roles.Where(x => x.ClientId == clientId || string.IsNullOrEmpty(x.ClientId));
            var roleClaims = await dataContext.RoleClaims.Join(filterdRoles,
                claim => claim.RoleId,
                role => role.Id,
                (claim, role) => new
                {
                    RoleName = role.Name,
                    RoleId = role.Id,
                    Claim = claim.ClaimType
                }).ToListAsync();
            var policies = roleClaims.GroupBy(x => x.RoleId, x => x)
                .Select(rolePolicies => new PoliciesViewModel
                {
                    RoleId = rolePolicies.Key,
                    RoleName = rolePolicies.First().RoleName,
                    Policies = rolePolicies.Select(x => x.Claim)
                }).ToList();
            policies.Insert(0, new PoliciesViewModel()
            {
                RoleId = Guid.Empty,
                RoleName = "",
                Policies = RoleClaims.GetClaims()
            });
            return policies;
        }
    }
}
