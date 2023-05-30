using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Antelcat.Foundation.Server.Extensions;

namespace Antelcat.Foundation.Server.Utils;

public class JwtConfigure<TIdentity> where TIdentity : class
{
    public JwtConfigure()
    {
        Secret = Guid.NewGuid().ToString();
    }
  
    public string Secret { set => SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value)); }

    private SecurityKey? SecurityKey
    {
        get => securityKey;
        set
        {
            securityKey = value;
            credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
    }
    private SecurityKey? securityKey;
    private SigningCredentials? credentials;

    private JwtSecurityToken GetToken(IEnumerable<Claim?> claims) => new(
        Parameters.ValidIssuer,
        Parameters.ValidAudience,
        claims,
        signingCredentials: credentials,
        notBefore: DateTime.Now);

    public TokenValidationParameters Parameters =>
        parameters ??= new TokenValidationParameters()
        {
            ValidIssuer = Assembly.GetExecutingAssembly().GetName().Name,
            ValidAudience = Assembly.GetExecutingAssembly().GetName().Name,
            ValidateIssuer = true,
            IssuerSigningKey = SecurityKey,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

    private readonly JwtSecurityTokenHandler handler = new();
    private TokenValidationParameters? parameters;
    public string? CreateToken(TIdentity source)
    {
        try
        {
            return handler.WriteToken(GetToken(source.GetClaims()));
        }
        catch
        {
            return null;
        }
    }
}