using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Entities.Shopping
{
    public class WishlistEntity : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<WishlistItemEntity> Items { get; set; }

        public WishlistEntity()
        {
            Items = new HashSet<WishlistItemEntity>();
            LastUpdated = DateTime.UtcNow;
        }
    }
}
