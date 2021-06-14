using FluentValidation;
using Shared.Models.ViewModels;

namespace TransactionAPI.Validators.v1
{
    public class PostTransactionValidator : AbstractValidator<PostTransaction>
    {
        public PostTransactionValidator()
        {
            RuleFor(c => c.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage("Description should not be blank");
            RuleFor(c => c.Description)
                .MinimumLength(2).WithMessage("The customer name must be at least 2 character long");
        }
    }
}