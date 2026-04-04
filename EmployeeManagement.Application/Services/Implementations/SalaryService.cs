using EmployeeManagement.Application.Abstractions.Repositories;
using EmployeeManagement.Application.DTO.Request;
using EmployeeManagement.Application.DTO.Response;
using EmployeeManagement.Application.Mappers;
using EmployeeManagement.Application.SalaryCalculation;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Shared;

namespace EmployeeManagement.Application.Services.Implementations;

public class SalaryService : ISalaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISalaryCalculationService _salaryCalculation;

    public SalaryService(IUnitOfWork unitOfWork, ISalaryCalculationService salaryCalculation)
    {
        _unitOfWork = unitOfWork;
        _salaryCalculation = salaryCalculation;
    }

    public async Task<ApiResult<IReadOnlyList<SalaryResponse>>> GetByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Employees.GetByIdAsync(employeeId, cancellationToken) is null)
            return ApiResult<IReadOnlyList<SalaryResponse>>.Fail("Employee not found.");

        var list = await _unitOfWork.Salaries.ListByEmployeeAsync(employeeId, cancellationToken);
        return ApiResult<IReadOnlyList<SalaryResponse>>.Ok(list.Select(SalaryMapper.ToResponse).ToList());
    }

    public async Task<ApiResult<IReadOnlyList<SalaryResponse>>> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        if (month is < 1 or > 12)
            return ApiResult<IReadOnlyList<SalaryResponse>>.Fail("Month must be between 1 and 12.");

        var list = await _unitOfWork.Salaries.ListByPeriodAsync(year, month, cancellationToken);
        return ApiResult<IReadOnlyList<SalaryResponse>>.Ok(list.Select(SalaryMapper.ToResponse).ToList());
    }

    public async Task<ApiResult<IReadOnlyList<SalaryResponse>>> DisburseAsync(DisburseSalaryRequest request, CancellationToken cancellationToken = default)
    {
        if (request.EmployeeIds.Count == 0)
            return ApiResult<IReadOnlyList<SalaryResponse>>.Fail("At least one employee is required.");

        if (request.Month is < 1 or > 12)
            return ApiResult<IReadOnlyList<SalaryResponse>>.Fail("Month must be between 1 and 12.");

        var created = new List<Salary>();

        foreach (var employeeId in request.EmployeeIds.Distinct())
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId, cancellationToken);
            if (employee is null)
                return ApiResult<IReadOnlyList<SalaryResponse>>.Fail($"Employee {employeeId} was not found.");

            if (!employee.IsActive)
                return ApiResult<IReadOnlyList<SalaryResponse>>.Fail($"Employee {employeeId} is not active.");

            if (await _unitOfWork.Salaries.ExistsForEmployeePeriodAsync(employeeId, request.Year, request.Month, cancellationToken))
                return ApiResult<IReadOnlyList<SalaryResponse>>.Fail(
                    $"Salary for employee {employeeId} already exists for {request.Month}/{request.Year}.");

            var adjustments = await _unitOfWork.SalaryAdjustments.ListForEmployeePeriodAsync(
                employeeId, request.Year, request.Month, cancellationToken);

            var net = _salaryCalculation.CalculateNetMonthlyPay(employee, request.Year, request.Month, adjustments);
            if (net <= 0)
                return ApiResult<IReadOnlyList<SalaryResponse>>.Fail(
                    $"Calculated net pay for employee {employeeId} must be greater than zero.");

            var adjustmentSum = adjustments.Sum(a => a.Amount);

            created.Add(new Salary
            {
                EmployeeId = employeeId,
                Amount = net,
                BaseComponent = employee.MonthlyBaseSalary,
                AdjustmentComponent = adjustmentSum,
                Month = request.Month,
                Year = request.Year,
                PaidDate = request.PaidDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(request.PaidDate, DateTimeKind.Utc)
                    : request.PaidDate.ToUniversalTime(),
                Reference = request.Reference
            });
        }

        _unitOfWork.Salaries.AddRange(created);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var withNames = new List<SalaryResponse>();
        foreach (var s in created)
        {
            var emp = await _unitOfWork.Employees.GetByIdAsync(s.EmployeeId, cancellationToken);
            withNames.Add(SalaryMapper.ToResponse(s, emp!.Name));
        }

        return ApiResult<IReadOnlyList<SalaryResponse>>.Ok(withNames);
    }

    public async Task<ApiResult<SalaryAdjustmentResponse>> AddAdjustmentAsync(CreateSalaryAdjustmentRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Month is < 1 or > 12)
            return ApiResult<SalaryAdjustmentResponse>.Fail("Month must be between 1 and 12.");

        var employee = await _unitOfWork.Employees.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
            return ApiResult<SalaryAdjustmentResponse>.Fail("Employee not found.");

        if (string.IsNullOrWhiteSpace(request.Reason))
            return ApiResult<SalaryAdjustmentResponse>.Fail("Reason is required.");

        var entity = new SalaryAdjustment
        {
            EmployeeId = request.EmployeeId,
            Year = request.Year,
            Month = request.Month,
            Amount = request.Amount,
            Reason = request.Reason.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        _unitOfWork.SalaryAdjustments.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResult<SalaryAdjustmentResponse>.Ok(new SalaryAdjustmentResponse(
            entity.Id,
            entity.EmployeeId,
            employee.Name,
            entity.Year,
            entity.Month,
            entity.Amount,
            entity.Reason,
            entity.CreatedAtUtc));
    }

    public async Task<ApiResult<IReadOnlyList<SalaryAdjustmentResponse>>> GetAdjustmentsByEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Employees.GetByIdAsync(employeeId, cancellationToken) is null)
            return ApiResult<IReadOnlyList<SalaryAdjustmentResponse>>.Fail("Employee not found.");

        var list = await _unitOfWork.SalaryAdjustments.ListForEmployeeAsync(employeeId, cancellationToken);
        return ApiResult<IReadOnlyList<SalaryAdjustmentResponse>>.Ok(list.Select(SalaryAdjustmentMapper.ToResponse).ToList());
    }
}
