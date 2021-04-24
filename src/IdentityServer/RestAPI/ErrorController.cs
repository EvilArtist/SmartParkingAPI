using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.RestAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        public IActionResult Error() 
        { 
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var message = exception.Error.Message;
            return Ok(message);
        }
    }
}
