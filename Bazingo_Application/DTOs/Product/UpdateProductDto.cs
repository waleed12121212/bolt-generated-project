using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Enums;
using System.Collections.Generic;

namespace Bazingo_Application.DTOs.Product
{
    public class UpdateProductDto
    {
        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(50)]
        public string? SKU { get; set; }

        [MaxLength(100)]
        public string? Brand { get; set; }

        public bool? IsFeatured { get; set; }

        public ProductStatus? Status { get; set; }

        public ICollection<string>? ImageUrls { get; set; }

        public IDictionary<string, string>? Attributes { get; set; }

        public UpdateProductDto()
        {
            ImageUrls = new List<string>();
            Attributes = new Dictionary<string, string>();
        }
    }
}
