using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartParking.Share.Constants;
using SmartParkingAbstract.Services.Admin;
using SmartParkingAbstract.ViewModels.General;
using SmartParkingAbstract.ViewModels.Users;
using SmartParkingCoreModels.Data;
using SmartParkingCoreModels.Identity;

namespace IdentityServer.RestAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService userService;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("employee")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<QueryResultModel<EmployeeViewModel>> GetUsers(string clientId)
        {
            return await userService.GetManagedUserAsync(clientId);
        }

        [HttpPost("employee/search")]
        public async Task<QueryResultModel<EmployeeViewModel>> SearchUsers(EmployeeQueryModel queryModel)
        {
            return await userService.SearchManagedUserAsync(queryModel);
        }

        [HttpPost("employee/admin")]
        [Authorize(RoleClaims.EmployeeManager)]
        public async Task<IdentityResult> CreateAdmin(AdminCreateModel model)
        {
            return await userService.CreateAdminAsync(model);
        }
    }
}
