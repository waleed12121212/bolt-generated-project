using Bazingo_Application.DTOs.Search;
using FluentValidation;

namespace Bazingo_Application.Validators.Search
{
    public class SearchDtoValidator : AbstractValidator<SearchDto>
    {
        private readonly string[] _validSortOptions = new[] 
        { 
            "name", 
            "price", 
            "rating", 
            "date", 
            "popularity" 
        };

        public SearchDtoValidator()
        {
            RuleFor(x => x.Query)
                .MaximumLength(100)
                .WithMessage("Search query cannot exceed 100 characters");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue)
                .WithMessage("Category ID must be greater than 0");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue)
                .WithMessage("Minimum price must be greater than or equal to 0");

            RuleFor(x => x.MaxPrice)
                .GreaterThan(0)
                .When(x => x.MaxPrice.HasValue)
                .WithMessage("Maximum price must be greater than 0");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice)
                .When(x => x.MaxPrice.HasValue && x.MinPrice.HasValue)
                .WithMessage("Maximum price must be greater than or equal to minimum price");

            RuleFor(x => x.MinRating)
                .InclusiveBetween(1, 5)
                .When(x => x.MinRating.HasValue)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.SortBy)
                .Must(x => string.IsNullOrEmpty(x) || _validSortOptions.Contains(x.ToLower()))
                .WithMessage($"Sort by must be one of: {string.Join(", ", _validSortOptions)}");

            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100");
        }
    }
}
