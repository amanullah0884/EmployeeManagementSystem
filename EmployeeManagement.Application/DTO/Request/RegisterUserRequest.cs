namespace EmployeeManagement.Application.DTO.Request;

public record RegisterUserRequest(string Username, string Password, int RoleId);
