using System.ComponentModel.DataAnnotations;

namespace Antelcat.Server.Test.Models
{
    public class User
    {
        [Required] public int Id { get; set; } = 10000;
        [Required] public string Name { get; set; } = "admin";
    }
}
