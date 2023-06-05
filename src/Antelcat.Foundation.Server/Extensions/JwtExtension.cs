using Antelcat.Foundation.Core.Extensions;
using Antelcat.Foundation.Core.Interface.Converting;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Antelcat.Foundation.Server.Extensions;

public static class JwtExtension<TIdentity> where TIdentity : class
{
    static JwtExtension()
    {
        var identityType = typeof(TIdentity);
            
        var props = identityType.GetProperties();
        ReadableProps = props
            .Where(static x => x.CanRead)
            .ToDictionary(
                static p => p.Name,
                static p => new Tuple<Getter<object, object>, IValueConverter>(
                    p.CreateGetter<object, object>(),
                    typeof(string).Converter(p.PropertyType)));

        WritableProps = props.Where(static x => x.CanWrite)
            .ToDictionary(
                static p => p.Name,
                static p => new Tuple<Setter<object, object>, IValueConverter>(
                    p.CreateSetter<object, object>(),
                    typeof(string).Converter(p.PropertyType)));
    }

    private static readonly IDictionary<string, Tuple<Getter<object, object>, IValueConverter>> ReadableProps;
    private static readonly IDictionary<string, Tuple<Setter<object, object>, IValueConverter>> WritableProps;
    private static TIdentity SetFromClaim(TIdentity identity, Claim claim)
    {
        if (!WritableProps.TryGetValue(claim.Type, out var tuple)) return identity;
        var i = (object)identity;
        tuple.Item1.Invoke(ref i!, tuple.Item2.To(claim.Value));
        return identity;
    }
    public static TIdentity? FromToken(TIdentity identity, string token)
    {
        try
        {
            return new JwtSecurityToken(token)
                .Claims
                .Aggregate(identity, SetFromClaim);
        }
        catch
        {
            return default;
        }
    }
    public static IEnumerable<Claim> GetClaims(TIdentity identity) =>
        ReadableProps
            .Select(x =>
                new Claim(x.Key, 
                    (string?)x.Value.Item2.From(x.Value.Item1.Invoke(identity)) ?? string.Empty));

    public static TIdentity FromClaims(TIdentity identity, IEnumerable<Claim> claims) =>
        claims.Aggregate(identity, SetFromClaim);
}

public static class JwtExtension
{
    public static TIdentity FromClaims<TIdentity>(this TIdentity identity, IEnumerable<Claim> claims)
        where TIdentity : class =>
        JwtExtension<TIdentity>.FromClaims(identity, claims);

    public static IEnumerable<Claim> GetClaims<TIdentity>(this TIdentity identity)
        where TIdentity : class =>
        JwtExtension<TIdentity>.GetClaims(identity);

    public static TIdentity? FromToken<TIdentity>(this TIdentity identity, string token)
        where TIdentity : class => 
        JwtExtension<TIdentity>.FromToken(identity, token);
  
}