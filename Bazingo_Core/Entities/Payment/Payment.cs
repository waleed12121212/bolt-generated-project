using System;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Enums;
using Bazingo_Core.Entities.Shopping;

namespace Bazingo_Core.Entities.Payment
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? RefundDate { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual OrderEntity Order { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Payment()
        {
            PaymentDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
        }
    }
}
