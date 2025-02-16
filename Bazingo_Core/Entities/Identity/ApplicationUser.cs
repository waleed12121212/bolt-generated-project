using Microsoft.AspNetCore.Identity;
using System;

namespace Bazingo_Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Enums.UserType UserType { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsVerified { get; set; } = false;

        public ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
