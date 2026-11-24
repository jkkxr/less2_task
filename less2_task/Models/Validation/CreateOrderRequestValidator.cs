using FluentValidation;
using less2_task.Models.DTO;

namespace less2_task.Models.Validation;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .NotNull().WithMessage("Quantity is required")
            .GreaterThan(0).WithMessage("Quantity must be greater than zero");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters");

        RuleFor(x => x)
            .CustomAsync(NeedComment);
    }
    private async Task NeedComment(
        CreateOrderRequest request,
        ValidationContext<CreateOrderRequest> context,
        CancellationToken cancellationToken
        )
    {
        if (request.Quantity > 10 && string.IsNullOrWhiteSpace(request.Comment.Trim()))
        {
            context.AddFailure(nameof(request.Comment),
                "Comment is required when product quantity is greater than 10.");
        }

        if (request.Price > 1000 && string.IsNullOrWhiteSpace(request.Comment.Trim()))
        {
            context.AddFailure(nameof(request.Comment),
                "Comment is required when product price exceeds 1000.");
        }
    }
}