using Bazingo_Application.DTOs.Cart;
using Bazingo_Core.Interfaces;
using FluentValidation;

namespace Bazingo_Application.Validators.Cart
{
    public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required")
                .GreaterThan(0).WithMessage("Valid product is required")
                .MustAsync(async (productId, cancellation) =>
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(productId);
                    return product != null && product.IsActive;
                }).WithMessage("Product not found or is not available");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100 items")
                .MustAsync(async (model, quantity, cancellation) =>
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(model.ProductId);
                    return product == null || product.StockQuantity >= quantity;
                }).WithMessage("Insufficient stock available");
        }
    }

    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.CartItemId)
                .NotEmpty().WithMessage("Cart item ID is required")
                .GreaterThan(0).WithMessage("Valid cart item is required");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required")
                .GreaterThan(0).WithMessage("Valid product is required")
                .MustAsync(async (productId, cancellation) =>
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(productId);
                    return product != null && product.IsActive;
                }).WithMessage("Product not found or is not available");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100 items")
                .MustAsync(async (model, quantity, cancellation) =>
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(model.ProductId);
                    return product == null || product.StockQuantity >= quantity;
                }).WithMessage("Insufficient stock available");
        }
    }
}
