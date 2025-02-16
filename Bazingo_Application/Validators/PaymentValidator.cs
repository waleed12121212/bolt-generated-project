using FluentValidation;
using Bazingo_Core.Entities.Payment;
using Bazingo_Core.Enums;

namespace Bazingo_Application.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Method)
                .IsInEnum().WithMessage("Invalid payment method.");

            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("Transaction ID is required.");

            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID must be valid.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid payment status.");

            RuleFor(x => x.PaymentDate)
                .NotEmpty().WithMessage("Payment date is required.");
        }
    }
}
