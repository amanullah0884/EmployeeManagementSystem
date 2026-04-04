using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Mappers;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IReadOnlyList<DepartmentResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Departments.ListAsync(cancellationToken);
        return ApiResult<IReadOnlyList<DepartmentResponse>>.Ok(list.Select(DepartmentMapper.ToResponse).ToList());
    }

    public async Task<ApiResult<DepartmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Departments.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<DepartmentResponse>.Fail("Department not found.");
        return ApiResult<DepartmentResponse>.Ok(DepartmentMapper.ToResponse(entity));
    }

    public async Task<ApiResult<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Departments.NameExistsAsync(request.Name, null, cancellationToken))
            return ApiResult<DepartmentResponse>.Fail("Department name already exists.");

        var entity = new Domain.Models.Department
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim()
        };

        _unitOfWork.Departments.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Departments.GetByIdAsync(entity.Id, cancellationToken);
        return ApiResult<DepartmentResponse>.Ok(DepartmentMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<DepartmentResponse>> UpdateAsync(int id, UpdateDepartmentRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Departments.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<DepartmentResponse>.Fail("Department not found.");

        if (await _unitOfWork.Departments.NameExistsAsync(request.Name, id, cancellationToken))
            return ApiResult<DepartmentResponse>.Fail("Department name already exists.");

        entity.Name = request.Name.Trim();
        entity.Description = request.Description.Trim();

        _unitOfWork.Departments.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Departments.GetByIdAsync(id, cancellationToken);
        return ApiResult<DepartmentResponse>.Ok(DepartmentMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Departments.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<bool>.Fail("Department not found.");

        if (entity.Employees.Count > 0)
            return ApiResult<bool>.Fail("Cannot delete a department that still has employees.");

        _unitOfWork.Departments.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResult<bool>.Ok(true);
    }
}
