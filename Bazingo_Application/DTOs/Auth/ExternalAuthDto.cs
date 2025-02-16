using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Models.Identity;

namespace Bazingo_Application.DTOs.Auth
{
    public class ExternalAuthDto : IExternalAuthModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string AccessToken { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class Enable2FADto
    {
        public string UserId { get; set; }
        public bool Enable { get; set; }
    }

    // Removed duplicate Verify2FADto class
}
