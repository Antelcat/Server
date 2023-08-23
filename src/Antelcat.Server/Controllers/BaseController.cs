using System.Security.Claims;
using Antelcat.Attributes;
using Antelcat.Core.Interface.Logging;
using Antelcat.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Controllers;

public abstract class BaseController<TCategory> : Controller
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class
        => ((identityCache ??=  typeof(TIdentity).RawInstance<TIdentity>().FromClaims(User.Claims)) as TIdentity)!;
    private object? identityCache;

    [Autowired] protected IAntelcatLogger<TCategory> Logger { get; init; } = null!;

    protected Task SignInAsync<TIdentity>(TIdentity identity, 
        string? authenticationType = "Identity.Application",
        AuthenticationProperties? properties = null,
        string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        where TIdentity : class
    {
        return Request.HttpContext.SignInAsync(scheme,
            new ClaimsPrincipal(new ClaimsIdentity(identity.GetClaims(), authenticationType)), properties);
    }

    protected Task SignOutAsync(string scheme = CookieAuthenticationDefaults.AuthenticationScheme) => 
        Request.HttpContext.SignOutAsync(scheme);
}
    
    
public abstract class BaseController<TIdentity,TCategory> : BaseController<TCategory> where TIdentity : class , new()
{
    protected TIdentity Identity => identity ??= new TIdentity().FromClaims(User.Claims);
    private TIdentity? identity;
}
