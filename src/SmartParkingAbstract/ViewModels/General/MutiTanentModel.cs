using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingAbstract.ViewModels.General
{
    public abstract class MultiTanentModel
    {
        public virtual Guid Id{ get; set; }
        public string ClientId { get; private set; }

        public MultiTanentModel()
        {

        }
        public MultiTanentModel(string clientId)
        {
            ClientId = clientId;
        }
        public virtual void GetClientIdFromContext(HttpContext context)
        {
            ClientId = context.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId).Value;
        }
    }
}
