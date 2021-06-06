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
using static SmartParking.Share.Constants.IdentityConstants;

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
        public async Task<QueryResultModel<EmployeeViewModel>> GetEmployees()
        {
            string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
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
            queryModel.GetClientIdFromContext(HttpContext);
            return await userService.SearchManagedUserAsync(queryModel);
        }

        [HttpPost("admin")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<ServiceResponse<EmployeeDetail>> CreateAdmin(AdminCreateModel model)
        {
            EmployeeCreateModel employeeCreateModel = new ()
            {
                Address = model.Address,
                Email = model.Email,
                FirstName = model.FirstName,
                IDCardNumber = model.IDCardNumber,
                LastName = model.LastName,
                Phone = model.Phone,

            };
            employeeCreateModel.GetClientIdFromContext(HttpContext);
            return await userService.CreateAdminAsync(employeeCreateModel);
        }

        [HttpPost("employee")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<ServiceResponse<EmployeeDetail>> CreateUsers(EmployeeCreateModel model)
        {
            model.GetClientIdFromContext(HttpContext);
            return await userService.CreateEmployeeAsync(model);
        }

        [HttpPost("employee/update/{userId}")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> UpdateUsers(Guid userId, EmployeeUpdateModel model)
        {
            model.GetClientIdFromContext(HttpContext);
            return await userService.UpdateEmployeeAsync(userId, model);
        }

        [HttpGet("policies")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IEnumerable<PoliciesViewModel>> GetPolicies()
        {
            string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
            return await userService.GetPoliciesAsync(clientId);
        }

        [HttpPost("policies/assign")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> AssignRolePolicyAsync(RolePolicyAssignmentViewModel assignment)
        {
            assignment.GetClientIdFromContext(HttpContext);
            return await userService.AssignRolePolicyAsync(assignment);
        }

        [HttpPost("policies/unassign")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> UnassignRolePolicyAsync(RolePolicyAssignmentViewModel assignment)
        {
            assignment.GetClientIdFromContext(HttpContext);
            return await userService.UnassignRolePolicyAsync(assignment);
        }

        [HttpGet("roles")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IEnumerable<RoleViewModel>> GetRoles()
        {
            string clientId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
            return await userService.GetRolesAsync(clientId);
        }

        [HttpPost("roles")]
        [Authorize(RoleClaims.RoleManager)]
        public async Task<IdentityResult> CreateRoleAsync(TanentRoleViewModel model)
        {
            model.GetClientIdFromContext(HttpContext);
            return await userService.CreateRoleAsync(model.RoleName);
        }

        [HttpPost("roles/remove")]
        [Authorize(RoleClaims.RoleManager)]
        public async Task<IdentityResult> RemoveRoleAsync(TanentRoleViewModel model)
        {
            model.GetClientIdFromContext(HttpContext);
            return await userService.RemoveRoleAsync(model.RoleName);
        }

        [HttpPost("delete")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> DeleteUserAsync(EmployeeDeleteModel model)
        {
            model.GetClientIdFromContext(HttpContext);
            return await userService.RemoveEmployeeAsync(model.UserId);
        }
    }
}
