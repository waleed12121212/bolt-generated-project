using System;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Entities.Payment
{
    public class EscrowTransaction : BaseEntity
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public decimal Amount { get; set; }
        public EscrowStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public virtual OrderEntity Order { get; set; }
        public virtual ApplicationUser Buyer { get; set; }
        public virtual ApplicationUser Seller { get; set; }

        public EscrowTransaction()
        {
            CreatedAt = DateTime.UtcNow;
            Status = EscrowStatus.Pending;
        }
    }

    public enum EscrowStatus
    {
        Pending = 1,
        Held = 2,
        Released = 3,
        Refunded = 4,
        Disputed = 5,
        Cancelled = 6
    }
}
