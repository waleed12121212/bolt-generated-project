using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.DTOs.Auth
{
    public class Verify2FADto
    {
        [Required]
        public string Code { get; set; }
        public bool RememberDevice { get; set; }
    }

    public class Verify2FADtoNew
    {
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }

        public bool RememberDevice { get; set; }
    }
}
