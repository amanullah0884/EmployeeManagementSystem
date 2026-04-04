using EmployeeManagement.Application.DTO.Request;
using FluentValidation;

namespace EmployeeManagement.Application.Validation;

public class UpdateDesignationRequestValidator : AbstractValidator<UpdateDesignationRequest>
{
    public UpdateDesignationRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
