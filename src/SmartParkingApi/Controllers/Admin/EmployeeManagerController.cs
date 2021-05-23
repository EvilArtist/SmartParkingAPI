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

        [HttpGet("policies")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IEnumerable<PoliciesViewModel>> GetPolicies(string clientId)
        {
            return await userService.GetPoliciesAsync(clientId);
        }

        [HttpPost("policies/assign")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> AssignRolePolicyAsync(RolePolicyAssignmentViewModel assignment)
        {
            return await userService.AssignRolePolicyAsync(assignment);
        }

        [HttpPost("policies/unassign")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> UnassignRolePolicyAsync(RolePolicyAssignmentViewModel assignment)
        {
            return await userService.UnassignRolePolicyAsync(assignment);
        }

        [HttpGet("roles")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IEnumerable<RoleViewModel>> GetRoles(string clientId)
        {
            return await userService.GetRolesAsync(clientId);
        }

        [HttpPost("roles")]
        [Authorize(RoleClaims.RoleManager)]
        public async Task<IdentityResult> CreateRoleAsync(TanentRoleViewModel model)
        {
            return await userService.CreateRoleAsync(model.RoleName);
        }

        [HttpPost("roles/remove")]
        [Authorize(RoleClaims.RoleManager)]
        public async Task<IdentityResult> RemoveRoleAsync(TanentRoleViewModel model)
        {
            return await userService.RemoveRoleAsync(model.RoleName);
        }

        [HttpPost("delete")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> DeleteUserAsync(EmployeeDeleteModel model)
        {
            return await userService.RemoveEmployeeAsync(model.UserId);
        }
    }
}
