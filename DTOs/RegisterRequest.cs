using System.ComponentModel.DataAnnotations;

namespace SafeVault.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "user";
    }
}
