using Microsoft.AspNetCore.Identity;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.Admin
{
    public interface IUserService
    {
        Task<QueryResultModel<EmployeeViewModel>> GetManagedUserAsync(string clientId);
        Task<EmployeeDetail> GetEmployeeById(Guid userId);
        Task<QueryResultModel<EmployeeViewModel>> SearchManagedUserAsync(EmployeeQueryModel queryModels);
        Task<ServiceResponse<EmployeeDetail>> CreateAdminAsync(EmployeeCreateModel model);
        Task<ServiceResponse<EmployeeDetail>> CreateEmployeeAsync(EmployeeCreateModel model);
        Task<IdentityResult> RemoveEmployeeAsync(Guid model);
        Task<IdentityResult> UpdateEmployeeAsync(Guid userId, EmployeeUpdateModel model);
        Task<IEnumerable<RoleViewModel>> GetRolesAsync(string clientId);
        Task<IEnumerable<PoliciesViewModel>> GetPoliciesAsync(string clientId);
        Task<IdentityResult> AssignRolePolicyAsync(RolePolicyAssignmentViewModel rolePolicyAssignment);
        Task<IdentityResult> UnassignRolePolicyAsync(RolePolicyAssignmentViewModel rolePolicyAssignment);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> RemoveRoleAsync(string roleName);
    }
}
