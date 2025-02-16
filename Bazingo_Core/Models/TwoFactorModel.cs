using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.Models
{
    public class TwoFactorModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
