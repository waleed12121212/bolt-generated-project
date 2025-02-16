using System;
using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Entities.Chat
{
    public class ChatMessageEntity : BaseEntity
    {
        [Required]
        public int ChatRoomId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }

        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Navigation properties
        public virtual ChatRoomEntity ChatRoom { get; set; }
        public virtual ApplicationUser Sender { get; set; }

        public ChatMessageEntity()
        {
            SentAt = DateTime.UtcNow;
        }
    }
}
