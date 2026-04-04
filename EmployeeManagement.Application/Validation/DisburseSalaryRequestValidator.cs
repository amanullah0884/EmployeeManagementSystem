using EmployeeManagement.Application.DTO.Request;
using FluentValidation;

namespace EmployeeManagement.Application.Validation;

public class DisburseSalaryRequestValidator : AbstractValidator<DisburseSalaryRequest>
{
    public DisburseSalaryRequestValidator()
    {
        RuleFor(x => x.EmployeeIds).NotEmpty();
        RuleForEach(x => x.EmployeeIds).GreaterThan(0);
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
        RuleFor(x => x.PaidDate).NotEmpty();
    }
}
