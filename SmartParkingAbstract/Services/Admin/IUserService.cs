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
        Task<QueryResultModel<EmployeeViewModel>> GetManagedUserAsync(string ClientId);
        Task<QueryResultModel<EmployeeViewModel>> GetManagedUserAsync(EmployeeQueryModel queryModels);
        Task<IdentityResult> CreateAdminAsync(AdminCreateModel model);
    }
}
