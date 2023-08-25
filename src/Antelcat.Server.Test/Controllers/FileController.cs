using System.Net.Mime;
using Antelcat.Server.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Test.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FileController : BaseController<FileController>
{
    [HttpGet("{fileName}")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Assets([FromRoute]string fileName = "Logo.png")
    {
        var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", fileName);
        return System.IO.File.Exists(path)
            ? new FileContentResult(await System.IO.File.ReadAllBytesAsync(path), MediaTypeNames.Image.Jpeg)
            : new NotFoundResult();
    }
}