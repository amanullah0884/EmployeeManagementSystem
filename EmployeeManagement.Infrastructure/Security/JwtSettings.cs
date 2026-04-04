namespace EmployeeManagement.Infrastructure.Security;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string Secret { get; set; } = null!;

    public int ExpiryMinutes { get; set; } = 60;
}
