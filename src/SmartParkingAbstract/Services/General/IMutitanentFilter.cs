using Microsoft.AspNetCore.Mvc;
using SmartParkingAbstract.ViewModels.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.Services.General
{
    public interface IMutitanentFilter
    {
        bool CheckRequest(ControllerBase controller, string clientId);
        bool CheckRequest(ControllerBase controller, MultiTanentModel model);
    }
}
