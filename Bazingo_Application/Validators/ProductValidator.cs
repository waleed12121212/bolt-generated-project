using FluentValidation;
using Bazingo_Core.Entities.Product;

namespace Bazingo_Application.Validators
{
    public class ProductValidator : AbstractValidator<ProductEntity>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
                .Matches("^[a-zA-Z0-9 -&]*$").WithMessage("Name can only contain letters, numbers, spaces, hyphens and ampersands");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThan(1000000).WithMessage("Price cannot exceed 1,000,000");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative")
                .LessThan(10000).WithMessage("Stock quantity cannot exceed 10,000");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters")
                .Matches("^[a-zA-Z0-9 -&]*$").WithMessage("Brand can only contain letters, numbers, spaces, hyphens and ampersands");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required")
                .GreaterThan(0).WithMessage("Please select a valid category");

            RuleFor(x => x.SellerId)
                .NotEmpty().WithMessage("Seller is required");

            RuleFor(x => x.Images)
                .Must(images => images == null || images.Count() <= 10)
                .WithMessage("Cannot have more than 10 images per product");

            RuleFor(x => x.IsAvailable)
                .NotNull().WithMessage("Availability status is required");
        }
    }
}
