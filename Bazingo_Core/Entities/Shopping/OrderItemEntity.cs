using Bazingo_Core.Entities.Product;

namespace Bazingo_Core.Entities.Shopping
{
    public class OrderItemEntity : BaseEntity
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
        
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }
}
