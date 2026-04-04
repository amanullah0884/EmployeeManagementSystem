using EmployeeManagement.Application.DTO.Request;
using FluentValidation;

namespace EmployeeManagement.Application.Validation;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(256).EmailAddress();
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.DesignationId).GreaterThan(0);
        RuleFor(x => x.MonthlyBaseSalary).GreaterThanOrEqualTo(0);
    }
}

