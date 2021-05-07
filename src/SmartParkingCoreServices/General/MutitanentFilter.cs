using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.Services.General;
using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingCoreServices.General
{
    public class MutitanentFilter : IMutitanentFilter
    {
        public bool CheckRequest(ControllerBase controller, string clientId)
        {
            var claims = controller.User.Claims.ToArray();
            //var clientId = claims[0].Value
            throw new NotImplementedException();
        }

        public bool CheckRequest(ControllerBase controller, MutiTanentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
