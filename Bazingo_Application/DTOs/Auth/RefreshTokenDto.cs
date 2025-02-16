using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
