using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Models.Identity;

namespace Bazingo_Application.DTOs.Auth
{
    public class LoginDto : ILoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string Username { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
