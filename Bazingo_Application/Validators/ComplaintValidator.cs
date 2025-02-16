using FluentValidation;
using Bazingo_Core.Entities;
using static Bazingo_Core.Entities.Complaint;

namespace Bazingo_Application.Validators
{
    public class ComplaintValidator : AbstractValidator<Complaint>
    {
        public ComplaintValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid complaint type.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid complaint status.");

            When(x => x.Type == ComplaintType.Order, () =>
            {
                RuleFor(x => x.OrderId)
                    .NotNull().WithMessage("Order ID is required for order complaints.")
                    .GreaterThan(0).WithMessage("Order ID must be valid.");
            });

            When(x => x.Type == ComplaintType.Product, () =>
            {
                RuleFor(x => x.ProductId)
                    .NotNull().WithMessage("Product ID is required for product complaints.")
                    .GreaterThan(0).WithMessage("Product ID must be valid.");
            });
        }
    }
}
