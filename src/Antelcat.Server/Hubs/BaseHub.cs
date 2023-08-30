using System.Security.Claims;
using Antelcat.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Hubs;

public abstract class BaseHub : Hub
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class
        => Identity(typeof(TIdentity).RawInstance<TIdentity>());
    
    protected TIdentity Identity<TIdentity>(TIdentity from) where TIdentity : class
        => ((identity ??= from.FromClaims(Context.User?.Claims ?? new List<Claim>())) as TIdentity)!;
    
    private object? identity;
}

public abstract class BaseHub<T> : Hub<T> where T : class
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class
        => Identity(typeof(TIdentity).RawInstance<TIdentity>());
    
    protected TIdentity Identity<TIdentity>(TIdentity from) where TIdentity : class
        => ((identity ??= from.FromClaims(Context.User?.Claims ?? new List<Claim>())) as TIdentity)!;

    private object? identity;
}