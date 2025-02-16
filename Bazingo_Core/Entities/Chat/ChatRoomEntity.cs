using System;
using System.Collections.Generic;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Entities.Chat
{
    public class ChatRoomEntity : BaseEntity
    {
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public int? ProductId { get; set; }
        public int? OrderId { get; set; }
        public ChatStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Navigation properties
        public virtual ApplicationUser Buyer { get; set; }
        public virtual ApplicationUser Seller { get; set; }
        public virtual Product.ProductEntity Product { get; set; }
        public virtual OrderEntity Order { get; set; }
        public virtual ICollection<ChatMessageEntity> Messages { get; set; }

        public ChatRoomEntity()
        {
            Messages = new HashSet<ChatMessageEntity>();
            CreatedAt = DateTime.UtcNow;
            Status = ChatStatus.Active;
        }
    }
}
