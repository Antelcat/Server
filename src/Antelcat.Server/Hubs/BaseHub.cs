using System.Security.Claims;
using Antelcat.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Hubs;

public abstract class BaseHub : Hub
{
    protected TIdentity Identity<TIdentity>() where TIdentity : class, new()
        => (TIdentity)(identity ??= new TIdentity().FromClaims(Context.User?.Claims ?? new List<Claim>()));
    private object? identity;
}