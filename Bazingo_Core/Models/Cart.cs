using System;
using System.Collections.Generic;
using Bazingo_Core.Entities.Shopping;

namespace Bazingo_Core.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<CartItemEntity> Items { get; set; }

        public Cart()
        {
            Items = new List<CartItemEntity>();
            LastUpdated = DateTime.UtcNow;
        }
    }
}
