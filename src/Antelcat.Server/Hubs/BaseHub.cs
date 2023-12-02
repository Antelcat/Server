using System.Security.Claims;
using Antelcat.Extensions;
using Antelcat.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Antelcat.Server.Hubs;

public abstract class BaseHub : Hub
{
    protected TIdentity Identity<TIdentity>() where TIdentity : IClaimSerializable, new()
    {
        var ret = new TIdentity();
        ret.FromClaims(Context.User?.Claims ?? new List<Claim>());
        return ret;
    }
}

public abstract class BaseHub<T> : Hub<T> where T : class
{
    protected TIdentity Identity<TIdentity>() where TIdentity : IClaimSerializable, new()
    {
        var ret = new TIdentity();
        ret.FromClaims(Context.User?.Claims ?? new List<Claim>());
        return ret;
    }
}