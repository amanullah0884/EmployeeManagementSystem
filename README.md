# Employee Management System (EMS)

.NET 9 ASP.NET Core Web API with clean architecture, JWT authentication, **database-driven RBAC** (User → Role → Permission), Entity Framework Core, and SQL Server.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB, Express, or full instance)
- Optional: [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) global tool for migrations

```bash
dotnet tool install --global dotnet-ef --version 9.0.14
```

## Project setup

1. Clone or extract the repository.
2. Restore packages:

```bash
dotnet restore
```

## Database configuration

1. Edit `EmployeeManagement.WebAPI/appsettings.json` (or user secrets / environment variables for production).

2. Set `ConnectionStrings:DefaultConnection` to your SQL Server instance, for example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Apply EF Core migrations (creates/updates schema):

```bash
dotnet ef database update --project EmployeeManagement.Persistence --startup-project EmployeeManagement.WebAPI
```

Or run the API once: migrations run automatically at startup via `Database.MigrateAsync()`.

## JWT configuration

In `appsettings.json`, configure the `Jwt` section:

- `Issuer`, `Audience`
- `Secret`: **must be a long, random string** in production (HMAC key)
- `ExpiryMinutes`

## Running the application

```bash
dotnet run --project EmployeeManagement.WebAPI
```

- HTTPS: follow the launch URL shown in the console (e.g. `https://localhost:7xxx`).
- Swagger UI (Development): `/swagger`

## Seeded data

On first run (empty permissions table), the database is seeded with:

- Permission rows (e.g. `employees.read`, `salaries.disburse`, …)
- Roles: **Admin**, **HR**, **Accountant**, **Viewer** with different permission sets
- Default admin user: **`admin`** / **`Admin@123`** — **change immediately in production**

## Authorization model (database-driven)

- Permissions are stored in the database and linked to roles (many-to-many).
- On login, the API loads the user’s role and permissions, issues a JWT containing **`permission` claims** (not framework role names alone).
- API endpoints use **`[Authorize(Policy = AppPermissions.…)]`**, which requires those JWT permission claims.

## API testing

1. Open Swagger (`/swagger` in Development).
2. **Login**: `POST /api/auth/login` with `{ "username": "admin", "password": "Admin@123" }`.
3. Copy the returned `token` value.
4. Click **Authorize** in Swagger, enter: `Bearer {your-token}`.
5. Call protected endpoints (employees, departments, salaries, reports, etc.).

**Logout**: `POST /api/auth/logout` (requires Authorization header). The client must delete the stored token; JWTs are stateless unless you add a server-side blocklist.

### Example: curl login

```bash
curl -s -X POST https://localhost:7xxx/api/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"admin\",\"password\":\"Admin@123\"}"
```

## Solution structure (high level)

- **EmployeeManagement.Domain** — entities and domain-only types
- **EmployeeManagement.Application** — DTOs, services, validators, salary calculation, reporting queries
- **EmployeeManagement.Persistence** — EF Core, repositories, unit of work, migrations, audit trail writer
- **EmployeeManagement.Infrastructure** — JWT, password hashing
- **EmployeeManagement.Shared** — shared API envelope, permissions constants
- **EmployeeManagement.Configuration** — DI composition
- **EmployeeManagement.WebAPI** — controllers, middleware (exception handling, request audit)

## Further documentation

- Salary disbursement uses **application-layer calculation**: `MonthlyBaseSalary` + monthly **adjustments** for the period.
- Reporting aggregates are implemented via **`IReportQueryService`** (separated from core CRUD services).
- Audit / activity logs support categories such as **Authentication**, **Http**, and **Error** (see `AuditLog` entity).

### Notable endpoints

| Area | Method | Route |
|------|--------|--------|
| Deactivate employee | PATCH | `/api/employees/{id}/deactivate` |
| Record salary adjustment | POST | `/api/salaries/adjustments` |
| List adjustments | GET | `/api/salaries/adjustments/employee/{employeeId}` |
| Disburse (calculated pay) | POST | `/api/salaries/disburse` |
| Total salary (period) | GET | `/api/reports/total-salary-disbursed/{year}/{month}` |
| Salary by department | GET | `/api/reports/salary-distribution-by-department/{year}/{month}` |
| Monthly summary | GET | `/api/reports/monthly-salary-summary/{year}/{month}` |
| Employee salary history | GET | `/api/reports/employee-salary-history/{employeeId}` |

Unhandled exceptions return a **structured JSON** body (`StandardResponse`) with `statusCode` **500** (see `ExceptionHandlingMiddleware`).
