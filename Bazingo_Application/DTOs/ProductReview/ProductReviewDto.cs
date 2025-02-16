using System;
using System.Collections.Generic;

namespace Bazingo_Application.DTOs.ProductReview
{
    public class ProductReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public int Helpful { get; set; }
    }

    public class CreateProductReviewDto
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }

    public class UpdateProductReviewDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }

    public class ProductReviewSummaryDto
    {
        public int ProductId { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int VerifiedPurchases { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new Dictionary<int, int>();
        public List<ProductReviewDto> TopReviews { get; set; } = new List<ProductReviewDto>();
    }
}
