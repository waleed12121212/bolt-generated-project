using System.ComponentModel.DataAnnotations;

namespace Bazingo_Core.Models.Identity
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
