using Antelcat.Attributes;
using Antelcat.Core.Extensions;
using Antelcat.Core.Interface.Logging;
using Antelcat.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Server.Controllers;

public abstract class BaseController<TCategory> : Controller
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class
        => (TIdentity)(identity ??= typeof(TIdentity).RawInstance().FromClaims(User.Claims));
    private object? identity;

    [Autowired] protected IAntelcatLogger<TCategory> Logger { get; init; } = null!;
}
    
    
public abstract class BaseController<TIdentity,TCategory> : BaseController<TCategory> where TIdentity : class , new()
{
    protected TIdentity Identity => identity ??= new TIdentity().FromClaims(User.Claims);
    private TIdentity? identity;
}
