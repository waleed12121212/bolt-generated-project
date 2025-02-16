using System;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Entities.Shopping
{
    public class OrderStatusHistoryEntity : BaseEntity
    {
        public OrderStatus Status { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Comment { get; set; }
        
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
    }
}
