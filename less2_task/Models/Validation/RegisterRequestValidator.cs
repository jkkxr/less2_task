using Microsoft.EntityFrameworkCore;
using FluentValidation;
using less2_task.Models.DTO;

namespace less2_task.Models.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(ApiDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
            .MustAsync(async (email, cancellationToken) =>
            {
                var emailExists = await dbContext.Users
                    .AnyAsync(u => u.Email.ToLower() == email.ToLower().Trim(), cancellationToken);
                return emailExists == false;
            }).WithMessage("Email is already in use");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
    }
}
