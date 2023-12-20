using System.Security.Claims;
using Antelcat.Attributes;
using Antelcat.ClaimSerialization;
using Antelcat.Core.Interface.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Controllers;

public abstract class BaseController<TCategory> : Controller
{
    protected TIdentity? Identity<TIdentity>() => ClaimSerializer.Deserialize<TIdentity>(User.Claims);

    [Autowired] protected IAntelcatLogger<TCategory> Logger { get; init; } = null!;

    protected Task SignInAsync<TIdentity>(TIdentity identity,
        string? authenticationType = "Identity.Application",
        AuthenticationProperties? properties = null,
        string scheme = CookieAuthenticationDefaults.AuthenticationScheme)

    {
        return Request.HttpContext.SignInAsync(scheme,
            new ClaimsPrincipal(new ClaimsIdentity(ClaimSerializer.Serialize(identity), authenticationType)),
            properties);
    }

    protected Task SignOutAsync(string scheme = CookieAuthenticationDefaults.AuthenticationScheme) =>
        Request.HttpContext.SignOutAsync(scheme);
}
