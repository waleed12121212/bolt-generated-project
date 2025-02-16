using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Models.Identity;

namespace Bazingo_Application.DTOs.Auth
{
    public class RegisterDto : IRegisterModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string UserType { get; set; } = "Seller";

        [Required]
        public int PreferredCurrencyId { get; set; }
    }
}
