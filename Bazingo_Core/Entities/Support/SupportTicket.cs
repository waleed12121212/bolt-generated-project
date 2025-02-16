using System;
using System.Collections.Generic;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Entities.Support
{
    public class SupportTicket : BaseEntity
    {
        public string UserId { get; set; }
        public int? OrderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string AssignedTo { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual OrderEntity Order { get; set; }
        public virtual ICollection<TicketMessage> Messages { get; set; }

        public SupportTicket()
        {
            Messages = new HashSet<TicketMessage>();
            CreatedAt = DateTime.UtcNow;
            Status = TicketStatus.Open;
            Priority = TicketPriority.Medium;
        }
    }

    public class TicketMessage : BaseEntity
    {
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsStaffReply { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual SupportTicket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

        public TicketMessage()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }

    public enum TicketStatus
    {
        Open = 1,
        InProgress = 2,
        WaitingForCustomer = 3,
        WaitingForThirdParty = 4,
        Resolved = 5,
        Closed = 6
    }

    public enum TicketPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }
}
