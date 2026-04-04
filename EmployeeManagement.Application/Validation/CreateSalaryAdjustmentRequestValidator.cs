using EmployeeManagement.Application.DTO.Request;
using FluentValidation;

namespace EmployeeManagement.Application.Validation;

public class CreateSalaryAdjustmentRequestValidator : AbstractValidator<CreateSalaryAdjustmentRequest>
{
    public CreateSalaryAdjustmentRequestValidator()
    {
        RuleFor(x => x.EmployeeId).GreaterThan(0);
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
