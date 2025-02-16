using FluentValidation;
using Bazingo_Core.Entities;

namespace Bazingo_Application.Validators
{
    public class CategoryValidator : AbstractValidator<Bazingo_Core.Entities.Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters")
                .Matches("^[a-zA-Z0-9 -]*$").WithMessage("Category name can only contain letters, numbers, spaces and hyphens");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(2000).WithMessage("Image URL cannot exceed 2000 characters")
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Please provide a valid URL for the image");

            RuleFor(x => x.ParentCategoryId)
                .Must((category, parentId) => !parentId.HasValue || parentId.Value != category.Id)
                .WithMessage("A category cannot be its own parent");
        }
    }
}
