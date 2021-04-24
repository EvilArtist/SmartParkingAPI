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
    public class EmployeeManagerService : IUserService
    {
        private readonly ApplicationIdentityContext dataContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IRandomGeneratorService randomGeneratorService;

        public EmployeeManagerService(ApplicationIdentityContext dataContext, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IRandomGeneratorService randomGeneratorService)
        {
            this.dataContext = dataContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.randomGeneratorService = randomGeneratorService;
        }

        [Authorize(Policy = RoleClaims.EmployeeManager)]
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
                    DisplayName = x.UserName,
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

        public async Task<QueryResultModel<EmployeeViewModel>> GetManagedUserAsync(EmployeeQueryModel queryModels)
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
                    DisplayName = x.UserName,
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
    }
}
