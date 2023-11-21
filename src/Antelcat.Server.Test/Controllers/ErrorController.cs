using System.Net;
using Antelcat.Server.Controllers;
using Antelcat.Server.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ErrorController : BaseController<ErrorController>
{
    [HttpGet]
    public async Task<IActionResult> Error()
    {
        throw (ServerException)(HttpStatusCode.Unauthorized, new
        Dictionary<string,string>
        {
            { "a", "1" }
        }, "This is an error defined");
    }
}