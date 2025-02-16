namespace Bazingo_Core.Interfaces.DTOs
{
    public interface ICartItemDto
    {
        int Id { get; set; }
        int ProductId { get; set; }
        string ProductName { get; set; }
        string ProductImage { get; set; }
        decimal UnitPrice { get; set; }
        int Quantity { get; set; }
        decimal Total { get; set; }
    }

    public interface IAddToCartDto
    {
        int ProductId { get; set; }
        int Quantity { get; set; }
    }

    public interface IUpdateCartItemDto
    {
        int CartItemId { get; set; }
        int Quantity { get; set; }
    }

    public interface ICartSummaryDto
    {
        decimal SubTotal { get; set; }
        decimal ShippingCost { get; set; }
        decimal Tax { get; set; }
        decimal Total { get; set; }
        int TotalItems { get; set; }
        IEnumerable<ICartItemDto> Items { get; set; }
    }
}
