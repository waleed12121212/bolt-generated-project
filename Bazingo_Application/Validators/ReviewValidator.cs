using FluentValidation;
using Bazingo_Core.Entities.Product;

namespace Bazingo_Application.Validators
{
    public class ReviewValidator : AbstractValidator<ProductReviewEntity>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Review content is required.")
                .MaximumLength(2000).WithMessage("Review content cannot exceed 2000 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");
        }
    }
}
