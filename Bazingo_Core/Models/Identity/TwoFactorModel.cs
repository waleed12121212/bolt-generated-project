    using System.ComponentModel.DataAnnotations;

    namespace Bazingo_Core.Models.Identity
    {
        public class TwoFactorModel : ITwoFactorModel
        {
            [Required]
            public string Code { get; set; }
            public string UserId { get; set; }
            public bool RememberDevice { get; set; }
        }
    }
