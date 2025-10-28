using FluentValidation;
using NuaSpa.Api.Models;

namespace NuaSpa.Api.Validation;

public class CreateServiceValidator : AbstractValidator<CreateServiceDto>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DurationMinutes).InclusiveBetween(10, 300);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class UpdateServiceValidator : AbstractValidator<UpdateServiceDto>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DurationMinutes).InclusiveBetween(10, 300);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class CreateServiceCategoryValidator : AbstractValidator<CreateServiceCategoryDto>
{
    public CreateServiceCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
    }
}

public class UpdateServiceCategoryValidator : AbstractValidator<UpdateServiceCategoryDto>
{
    public UpdateServiceCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
    }
}
