using System.Security.Claims;
using Antelcat.Core.Extensions;
using Antelcat.Extensions;
using Microsoft.AspNetCore.SignalR;
using TypeExtension = Antelcat.Core.Extensions.TypeExtension;

namespace Antelcat.Server.Hubs;

public abstract class BaseHub : Hub
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class
        => (TIdentity)(identity ??= typeof(TIdentity).RawInstance().FromClaims(Context.User?.Claims ?? new List<Claim>()));
    private object? identity;
}

public abstract class BaseHub<T> : Hub<T> where T : class
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class
        => (TIdentity)(identity ??= TypeExtension.RawInstance<TIdentity>().FromClaims(Context.User?.Claims ?? new List<Claim>()));
    
    private object? identity;
}