using Antelcat.Attributes;
using Antelcat.Core.Attributes;
using Antelcat.Core.Extensions;
using Antelcat.Core.Models;
using Antelcat.Server.Controllers;
using Antelcat.Server.Test.Models;
using Antelcat.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers
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
            Logger.LogTrace($"{Identity<User>()}");
            return Identity<User>();
        }
    }
}
