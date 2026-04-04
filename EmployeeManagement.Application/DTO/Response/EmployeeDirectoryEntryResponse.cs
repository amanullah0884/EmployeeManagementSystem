namespace EmployeeManagement.Application.DTO.Response;

public record EmployeeDirectoryEntryResponse(
    int EmployeeId,
    string Name,
    string Email,
    string Department,
    string Designation,
    bool IsActive);
