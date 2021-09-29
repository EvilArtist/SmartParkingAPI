using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartParking.Share.Constants.IdentityConstants;

namespace SmartParkingCoreServices.General
{
    public class MultitanentService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MultitanentService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        protected string GetClientId()
        {
            if (httpContextAccessor != null )
            {
                var claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ClientId);

                var clientId = claim?.Value ?? "";
                return clientId;
            } 
            else
            {
                return "";
            }
        }

        protected Guid GetCurrentUserId()
        {
            return Guid.Parse(httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Id).Value);
        }
    }
}
