using System.Security.Claims;
using Antelcat.Attributes;
using Antelcat.Core.Extensions;
using Antelcat.Core.Models;
using Antelcat.Extensions;
using Antelcat.Server.Controllers;
using Antelcat.Server.Test.Models;
using Antelcat.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : BaseController<HomeController>
    {
        [Autowired] private readonly JwtConfigure<User> configure = null!;


        [HttpPost(nameof(JwtLogin))]
        [AllowAnonymous]
        public Response<string> JwtLogin([FromBody]User user)
        {
            var token = configure.CreateToken(user)!;
            Response.Cookies.Append("Authorization", $"Bearer {token}");
            return new Response<string>(token);
        }
        
        [HttpPost(nameof(CookieLogin))]
        [AllowAnonymous]
        public async Task<Response> CookieLogin([FromBody]User user)
        {
            await SignInAsync(user, "User",
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.MaxValue
                });
            return (1, "登录成功");
        }
        
        [HttpGet(nameof(CookieLogout))]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<Response> CookieLogout()
        {
            await SignOutAsync();
            return (1, "登出成功");
        }
        
        [HttpGet(nameof(WhoAmICookie))]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public Response<User> WhoAmICookie()
        {
            var user = Identity<User>();
            Logger.LogTrace($"{user.Serialize()}");
            return user;
        }
        
        [HttpGet(nameof(WhoAmIJwt))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Response<User> WhoAmIJwt()
        {
            var user = Identity<User>();
            Logger.LogTrace($"{user.Serialize()}");
            return user;
        }
    }
}
