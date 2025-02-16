using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Review;

namespace Bazingo_Core.DomainLogic
{
    public class ReviewManager
    {
        public bool ValidateReview(ProductReview review)
        {
            if (review == null)
                return false;

            if (string.IsNullOrWhiteSpace(review.Comment))
                return false;

            if (review.Rating < 1 || review.Rating > 5)
                return false;

            return true;
        }

        public double CalculateAverageRating(IEnumerable<ProductReview> reviews)
        {
            if (reviews == null || !reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }

        public Dictionary<int, int> GetRatingDistribution(IEnumerable<ProductReview> reviews)
        {
            var distribution = new Dictionary<int, int>();

            for (int i = 1; i <= 5; i++)
                distribution[i] = reviews?.Count(r => r.Rating == i) ?? 0;

            return distribution;
        }

        public IEnumerable<ProductReview> GetTopReviews(IEnumerable<ProductReview> reviews, int count = 5)
        {
            return reviews?
                .OrderByDescending(r => r.Helpful)
                .ThenByDescending(r => r.CreatedAt)
                .Take(count) ?? Enumerable.Empty<ProductReview>();
        }

        public bool HasUserReviewed(IEnumerable<ProductReview> reviews, string userId)
        {
            return reviews?.Any(r => r.UserId == userId) ?? false;
        }

        public IEnumerable<ProductReview> FilterReviews(IEnumerable<ProductReview> reviews, int? minRating = null, 
            int? maxRating = null, bool? verified = null)
        {
            var query = reviews.AsQueryable();

            if (minRating.HasValue)
                query = query.Where(r => r.Rating >= minRating.Value);

            if (maxRating.HasValue)
                query = query.Where(r => r.Rating <= maxRating.Value);

            if (verified.HasValue)
                query = query.Where(r => r.IsVerifiedPurchase == verified.Value);

            return query;
        }
    }
}
