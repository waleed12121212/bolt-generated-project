using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.Models
{
    public class ExternalAuthModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string AccessToken { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
    }
}
