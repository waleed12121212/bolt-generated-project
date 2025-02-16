using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Entities
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Navigation property
        public virtual ApplicationUser User { get; set; }
    }

    public enum NotificationType
    {
        OrderStatus = 1,
        BidUpdate = 2,
        ProductApproval = 3,
        Review = 4,
        Refund = 5,
        System = 6,
        Chat = 7
    }

    public class NotificationTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
