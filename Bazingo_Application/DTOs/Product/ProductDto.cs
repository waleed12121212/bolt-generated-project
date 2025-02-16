using System;
using System.Collections.Generic;
using Bazingo_Core.Enums;

namespace Bazingo_Application.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Brand { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int ViewCount { get; set; }
        public double AverageRating { get; set; }
        public ProductStatus Status { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<string> ImageUrls { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public int ReviewsCount { get; set; }

        public ProductDto()
        {
            ImageUrls = new List<string>();
            Attributes = new Dictionary<string, string>();
        }
    }
}
