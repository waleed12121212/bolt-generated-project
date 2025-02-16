using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Entities.Shopping
{
    public class CartEntity : BaseEntity
    {
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CartItemEntity> Items { get; set; }

        public CartEntity()
        {
            Items = new HashSet<CartItemEntity>();
            LastUpdated = DateTime.UtcNow;
        }
    }
}
