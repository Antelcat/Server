using Antelcat.Attributes;
using Antelcat.Core.Interface.Logging;
using Antelcat.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Foundation.Server.Controllers;

public abstract class BaseController : Controller
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class, new()
        => (TIdentity)(identity ??= new TIdentity().FromClaims(User.Claims));
    private object? identity;

    [Autowired] protected IAntelcatLogger<BaseController> Logger { get; init; } = null!;
}
    
    
public abstract class BaseController<TIdentity> : BaseController where TIdentity : class , new()
{
    protected TIdentity Identity => identity ??= new TIdentity().FromClaims(User.Claims);
    private TIdentity? identity;
}
