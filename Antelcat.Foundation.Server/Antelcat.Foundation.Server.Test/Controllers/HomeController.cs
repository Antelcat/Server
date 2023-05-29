using System.Reflection;
using System.Security.Claims;
using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Models;
using Antelcat.Foundation.Server.Controllers;
using Antelcat.Foundation.Server.Extensions;
using Antelcat.Foundation.Server.Utils;
using Feast.Foundation.Server.Test.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feast.Foundation.Server.Test.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]

    public class HomeController : BaseController<User>
    {
        private JwtConfigure<User> configure;

        public HomeController(JwtConfigure<User> configure)
        {
            this.configure = configure;
        }

        [HttpPost]
        [AllowAnonymous]
        public Response<string> Login([FromBody]User user)
        {
            var token = configure.CreateToken(user)!;
            Response.Cookies.Append("Authorization", $"Bearer {token}");
            return new Response<string>(token);
        }
        
        [HttpPost]
        public Response<User> WhoAmI()
        {
            Logger.LogTrace($"{Identity}");
            return Identity;
        }
    }
}
