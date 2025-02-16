using Bazingo_Application.DTOs.Shipping;
using Bazingo_Core.Entities.Shopping;
using FluentValidation;

namespace Bazingo_Application.Validators
{
    public class ShippingValidator : AbstractValidator<OrderEntity>
    {
        public ShippingValidator()
        {
            RuleFor(x => x.ShippingAddress)
                .NotEmpty()
                .WithMessage("Shipping address is required");

            RuleFor(x => x.TrackingNumber)
                .NotEmpty()
                .When(x => x.Status == Bazingo_Core.Enums.OrderStatus.Shipped)
                .WithMessage("Tracking number is required for shipped orders");
        }
    }
}
