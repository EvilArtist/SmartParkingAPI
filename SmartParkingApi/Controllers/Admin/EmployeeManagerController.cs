using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Admin;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParkingApi.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeManagerController : ControllerBase
    {
        private readonly IUserService userService;

        public EmployeeManagerController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("employee")]
        [Authorize(RoleClaims.EmployeeView)]
        public async Task<QueryResultModel<EmployeeViewModel>> GetEmployees(string clientId)
        {
            return await userService.GetManagedUserAsync(clientId);
        }

        [HttpGet("employee/{userId}")]
        [Authorize(RoleClaims.EmployeeView)]
        public async Task<EmployeeDetail> GetEmployee(Guid userId)
        {
            return await userService.GetEmployeeById(userId);
        }

        [HttpPost("search")]
        public async Task<QueryResultModel<EmployeeViewModel>> SearchUsers(EmployeeQueryModel queryModel)
        {
            return await userService.SearchManagedUserAsync(queryModel);
        }

        [HttpPost("admin")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> CreateAdmin(AdminCreateModel model)
        {
            return await userService.CreateAdminAsync(model);
        }

        [HttpPost("employee")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> CreateUsers(EmployeeCreateModel model)
        {
            return await userService.CreateEmployeeAsync(model);
        }

        [HttpPost("employee/update/{userId}")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> UpdateUsers(Guid userId, EmployeeUpdateModel model)
        {
            return await userService.UpdateEmployeeAsync(userId, model);
        }

        [HttpGet("roles")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IEnumerable<RoleViewModel>> GetRoles(string clientId)
        {
            return await userService.GetRolesAsync(clientId);
        }

        [HttpGet("policies")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IEnumerable<PoliciesViewModel>> GetPolicies(string clientId)
        {
            return await userService.GetPoliciesAsync(clientId);
        }
    }
}
