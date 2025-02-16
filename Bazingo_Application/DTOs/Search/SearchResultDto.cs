using System.Collections.Generic;

namespace Bazingo_Application.DTOs.Search
{
    public class SearchResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public Dictionary<string, object> Facets { get; set; } = new();
        public SearchMetadataDto Metadata { get; set; }
    }
}
