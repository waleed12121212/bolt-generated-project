    namespace Bazingo_Core.Settings
    {
        public class JwtSettings
        {
            [Required(ErrorMessage = "JWT Key is required")]
            [MinLength(16, ErrorMessage = "JWT Key must be at least 16 characters long")]
            public string Key { get; set; }

            [Required(ErrorMessage = "JWT Issuer is required")]
            public string Issuer { get; set; }

            [Required(ErrorMessage = "JWT Audience is required")]
            public string Audience { get; set; }

            [Required(ErrorMessage = "JWT Expiration time is required")]
            [Range(5, 1440, ErrorMessage = "Expiration time must be between 5 and 1440 minutes")]
            public int ExpirationMinutes { get; set; } = 60;

            public string TokenType { get; set; } = "Bearer";

            public bool ValidateIssuerSigningKey { get; set; } = true;
            public bool ValidateIssuer { get; set; } = true;
            public bool ValidateAudience { get; set; } = true;
            public bool ValidateLifetime { get; set; } = true;
            public int ClockSkew { get; set; } = 5;
        }
    }
