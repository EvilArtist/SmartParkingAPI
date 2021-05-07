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
        Task<IdentityResult> CreateAdminAsync(AdminCreateModel model);
        Task<IdentityResult> CreateEmployeeAsync(EmployeeCreateModel model);
        Task<IdentityResult> UpdateEmployeeAsync(Guid userId, EmployeeUpdateModel model);
        Task<IEnumerable<RoleViewModel>> GetRolesAsync(string clientId);
        Task<IEnumerable<PoliciesViewModel>> GetPoliciesAsync(string clientId);
     }
}
