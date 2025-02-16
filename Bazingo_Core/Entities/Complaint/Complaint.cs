using System;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Entities.Product;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Entities
{
    public class Complaint : BaseEntity
    {
        public string UserId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ComplaintStatus Status { get; set; }
        public ComplaintType Type { get; set; }
        public string Resolution { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual Shopping.OrderEntity Order { get; set; }
        public virtual ProductEntity Product { get; set; }
    }

    public enum ComplaintStatus
    {
        Pending = 1,
        InProgress = 2,
        Resolved = 3,
        Rejected = 4
    }

    public enum ComplaintType
    {
        Order = 1,
        Product = 2,
        Delivery = 3,
        Service = 4,
        Other = 5
    }
}
