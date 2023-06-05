using Antelcat.Attributes;
using Antelcat.Core.Extensions;
using Antelcat.Core.Models;
using Antelcat.Foundation.Server.Controllers;
using Antelcat.Utils;
using Feast.Foundation.Server.Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feast.Foundation.Server.Test.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class HomeController : BaseController<User>
    {
        [Autowired] private readonly JwtConfigure<User> configure = null!;


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
