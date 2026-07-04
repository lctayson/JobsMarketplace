using FluentValidation;
using JobsMarketplace.API.Dtos;

namespace JobsMarketplace.API.Validators;

public class JobValidator : AbstractValidator<JobDto>
{
    public JobValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Budget).GreaterThan(0);
        RuleFor(x => x.DueDate).GreaterThan(x => x.StartDate)
            .WithMessage("Due date must be after start date.");
    }
}