using EmployeeManagement.Application.DTO.Request;
using FluentValidation;

namespace EmployeeManagement.Application.Validation;

public class CreateDesignationRequestValidator : AbstractValidator<CreateDesignationRequest>
{
    public CreateDesignationRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
