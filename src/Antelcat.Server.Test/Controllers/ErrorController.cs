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
        throw (RejectException)(HttpStatusCode.Unauthorized,
            new
            {
                code = 0,
                message = "Error occurred"
            }
            , "This is an error defined");
    }
}