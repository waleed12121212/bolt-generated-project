using Bazingo_Application.DTOs.ProductReview;
using FluentValidation;

namespace Bazingo_Application.Validators.ProductReview
{
    public class CreateProductReviewDtoValidator : AbstractValidator<CreateProductReviewDto>
    {
        public CreateProductReviewDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(200)
                .WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Comment is required")
                .MinimumLength(10)
                .WithMessage("Comment must be at least 10 characters long")
                .MaximumLength(2000)
                .WithMessage("Comment cannot exceed 2000 characters");
        }
    }
}
