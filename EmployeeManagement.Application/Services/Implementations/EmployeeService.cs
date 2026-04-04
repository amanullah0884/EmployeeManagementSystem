using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Mappers;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IReadOnlyList<EmployeeResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Employees.ListAsync(cancellationToken);
        return ApiResult<IReadOnlyList<EmployeeResponse>>.Ok(list.Select(EmployeeMapper.ToResponse).ToList());
    }

    public async Task<ApiResult<EmployeeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<EmployeeResponse>.Fail("Employee not found.");
        return ApiResult<EmployeeResponse>.Ok(EmployeeMapper.ToResponse(entity));
    }

    public async Task<ApiResult<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Employees.EmailExistsAsync(request.Email, null, cancellationToken))
            return ApiResult<EmployeeResponse>.Fail("Email is already in use.");

        if (await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken) is null)
            return ApiResult<EmployeeResponse>.Fail("Department not found.");

        if (await _unitOfWork.Designations.GetByIdAsync(request.DesignationId, cancellationToken) is null)
            return ApiResult<EmployeeResponse>.Fail("Designation not found.");

        var entity = new Domain.Models.Employee
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            DepartmentId = request.DepartmentId,
            DesignationId = request.DesignationId,
            IsActive = true,
            MonthlyBaseSalary = request.MonthlyBaseSalary,
            CreatedAt = DateTime.UtcNow
        };

        _unitOfWork.Employees.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Employees.GetByIdAsync(entity.Id, cancellationToken);
        return ApiResult<EmployeeResponse>.Ok(EmployeeMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<EmployeeResponse>> UpdateAsync(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<EmployeeResponse>.Fail("Employee not found.");

        if (await _unitOfWork.Employees.EmailExistsAsync(request.Email, id, cancellationToken))
            return ApiResult<EmployeeResponse>.Fail("Email is already in use.");

        if (await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId, cancellationToken) is null)
            return ApiResult<EmployeeResponse>.Fail("Department not found.");

        if (await _unitOfWork.Designations.GetByIdAsync(request.DesignationId, cancellationToken) is null)
            return ApiResult<EmployeeResponse>.Fail("Designation not found.");

        entity.Name = request.Name.Trim();
        entity.Email = request.Email.Trim();
        entity.Phone = request.Phone.Trim();
        entity.DepartmentId = request.DepartmentId;
        entity.DesignationId = request.DesignationId;
        entity.IsActive = request.IsActive;
        entity.MonthlyBaseSalary = request.MonthlyBaseSalary;

        _unitOfWork.Employees.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        return ApiResult<EmployeeResponse>.Ok(EmployeeMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<EmployeeResponse>> DeactivateAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<EmployeeResponse>.Fail("Employee not found.");

        entity.IsActive = false;
        _unitOfWork.Employees.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        return ApiResult<EmployeeResponse>.Ok(EmployeeMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Employees.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<bool>.Fail("Employee not found.");

        _unitOfWork.Employees.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResult<bool>.Ok(true);
    }
}
