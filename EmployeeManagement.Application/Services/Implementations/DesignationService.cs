using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Mappers;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

public class DesignationService : IDesignationService
{
    private readonly IUnitOfWork _unitOfWork;

    public DesignationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IReadOnlyList<DesignationResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _unitOfWork.Designations.ListAsync(cancellationToken);
        return ApiResult<IReadOnlyList<DesignationResponse>>.Ok(list.Select(DesignationMapper.ToResponse).ToList());
    }

    public async Task<ApiResult<DesignationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Designations.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<DesignationResponse>.Fail("Designation not found.");
        return ApiResult<DesignationResponse>.Ok(DesignationMapper.ToResponse(entity));
    }

    public async Task<ApiResult<DesignationResponse>> CreateAsync(CreateDesignationRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Designations.NameExistsAsync(request.Name, null, cancellationToken))
            return ApiResult<DesignationResponse>.Fail("Designation name already exists.");

        var entity = new Domain.Models.Designation { Name = request.Name.Trim() };

        _unitOfWork.Designations.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Designations.GetByIdAsync(entity.Id, cancellationToken);
        return ApiResult<DesignationResponse>.Ok(DesignationMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<DesignationResponse>> UpdateAsync(int id, UpdateDesignationRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Designations.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<DesignationResponse>.Fail("Designation not found.");

        if (await _unitOfWork.Designations.NameExistsAsync(request.Name, id, cancellationToken))
            return ApiResult<DesignationResponse>.Fail("Designation name already exists.");

        entity.Name = request.Name.Trim();

        _unitOfWork.Designations.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity = await _unitOfWork.Designations.GetByIdAsync(id, cancellationToken);
        return ApiResult<DesignationResponse>.Ok(DesignationMapper.ToResponse(entity!));
    }

    public async Task<ApiResult<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Designations.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return ApiResult<bool>.Fail("Designation not found.");

        if (entity.Employees.Count > 0)
            return ApiResult<bool>.Fail("Cannot delete a designation that is still assigned to employees.");

        _unitOfWork.Designations.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResult<bool>.Ok(true);
    }
}
