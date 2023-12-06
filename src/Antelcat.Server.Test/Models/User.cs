using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Antelcat.ClaimSerialization.Attributes;
using Antelcat.ClaimSerialization.Interfaces;

namespace Antelcat.Server.Test.Models;

[ClaimSerializable]
public partial class User : IClaimSerializable
{
    [Required] public int Id { get; set; } = 10000;
    [Required] public string Name { get; set; } = "admin";
    [Required] [ClaimType(ClaimTypes.Role)] public string Role { get; set; } = "Doctor";
}