using System.Collections.Generic;

namespace Bazingo_Application.DTOs.Search
{
    public class SearchMetadataDto
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalBrands { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<string> PopularSearchTerms { get; set; } = new();
        public Dictionary<string, int> CategoryDistribution { get; set; } = new();
        public Dictionary<string, int> BrandDistribution { get; set; } = new();
    }
}
