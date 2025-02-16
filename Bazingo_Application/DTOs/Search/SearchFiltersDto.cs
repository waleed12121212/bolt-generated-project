using System.Collections.Generic;

namespace Bazingo_Application.DTOs.Search
{
    public class SearchFiltersDto
    {
        public List<string> Categories { get; set; } = new();
        public List<string> Brands { get; set; } = new();
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
