namespace EmployeeManagement.Application.DTO.Response;

public record LoginResponse(string Token, string Username, string Role, IReadOnlyList<string> Permissions);
