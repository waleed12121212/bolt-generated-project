using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Entities.Product;

namespace Bazingo_Core.Entities.Shopping
{
    public class CartItemEntity : BaseEntity
    {
        [Required]
        public int CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        // Navigation properties
        public virtual CartEntity Cart { get; set; }
        public virtual ProductEntity Product { get; set; }

        public void UpdateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;
        }
    }
}
