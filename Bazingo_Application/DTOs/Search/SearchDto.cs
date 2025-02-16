namespace Bazingo_Application.DTOs.Search
{
    public class SearchDto
    {
        public string Query { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinRating { get; set; }
        public bool? InStock { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
