using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Antelcat.ClaimSerialization.ComponentModel;

namespace Antelcat.Server.Test.Models;

public partial class User
{
    [Required] public int Id { get; set; } = 10000;
    [Required] public string Name { get; set; } = "admin";
    [Required] [ClaimType(ClaimTypes.Role)] public string Role { get; set; } = "Doctor";
}