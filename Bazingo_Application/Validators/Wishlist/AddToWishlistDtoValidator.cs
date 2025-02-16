using Bazingo_Application.DTOs.Wishlist;
using FluentValidation;

namespace Bazingo_Application.Validators.Wishlist
{
    public class AddToWishlistDtoValidator : AbstractValidator<AddToWishlistDto>
    {
        public AddToWishlistDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");
        }
    }
}
