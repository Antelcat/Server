using System.Security.Claims;
using Antelcat.Attributes;
using Antelcat.Core.Interface.Logging;
using Antelcat.Extensions;
using Antelcat.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Controllers;

public abstract class BaseController<TCategory> : Controller
{
    protected TIdentity Identity<TIdentity>() where TIdentity : IClaimSerializable, new()
    {
        var ret = new TIdentity();
        ret.FromClaims(User.Claims);
        return ret;
    }

    [Autowired] protected IAntelcatLogger<TCategory> Logger { get; init; } = null!;

    protected Task SignInAsync<TIdentity>(TIdentity identity,
        string? authenticationType = "Identity.Application",
        AuthenticationProperties? properties = null,
        string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        where TIdentity : IClaimSerializable, new()

    {
        return Request.HttpContext.SignInAsync(scheme,
            new ClaimsPrincipal(new ClaimsIdentity(identity.ToClaims(), authenticationType)), properties);
    }

    protected Task SignOutAsync(string scheme = CookieAuthenticationDefaults.AuthenticationScheme) =>
        Request.HttpContext.SignOutAsync(scheme);
}
