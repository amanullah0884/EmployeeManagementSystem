# Employee Management System (EMS) — Backend

ASP.NET Core Web API (.NET 9) implementing a layered Employee Management System with JWT authentication, database-driven permissions, dual databases (SQL Server + PostgreSQL), salary disbursement, reporting, FluentValidation, and structured API responses.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- **Microsoft SQL Server** (LocalDB, Express, or full instance) for application data
- **PostgreSQL** 14+ for audit and activity logs

## Solution structure

| Project | Responsibility |
|--------|----------------|
| `EmployeeManagement.WebAPI` | Controllers, middleware |
| `EmployeeManagement.Application` | Services, DTOs, validators, mappers, reporting queries |
| `EmployeeManagement.Domain` | Entities, enums (no infrastructure) |
| `EmployeeManagement.Persistence` | EF Core contexts, repositories, unit of work, migrations |
| `EmployeeManagement.Infrastructure` | JWT, password hashing |
| `EmployeeManagement.Shared` | API response models, exceptions, permission names |
| `EmployeeManagement.Configuration` | DI registration |

## Database configuration

Edit `EmployeeManagement.WebAPI/appsettings.json` (or use user secrets / environment variables):

1. **`ConnectionStrings:DefaultConnection`** — SQL Server connection string for main data (employees, departments, salaries, users, roles, permissions).

2. **`ConnectionStrings:LoggingConnection`** — PostgreSQL Npgsql-style connection string for logs (audit trail, auth events, HTTP audit, errors).

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;",
  "LoggingConnection": "Host=localhost;Port=5432;Database=EmployeeManagementLogs;Username=postgres;Password=postgres"
}
```

Create empty databases if your server does not create them automatically (SQL Server often does on first migrate; ensure PostgreSQL database `EmployeeManagementLogs` exists or allow the app user to create it).

## JWT

Configure the `Jwt` section in `appsettings.json`:

- `Issuer`, `Audience`, `Secret` (use a long random secret in production), `ExpiryMinutes`.

## Running the application

From the repository root:

```bash
dotnet run --project EmployeeManagement.WebAPI
```

On startup the API:

1. Applies **SQL Server** EF migrations (`AppDbContext`).
2. Applies **PostgreSQL** EF migrations (`LoggingDbContext`).
3. Seeds permissions, roles, and a default admin user (only when the database has no permissions yet).

**Development URL:** `https://localhost:7xxx` (see console output). Swagger UI is enabled in the Development environment at `/swagger`.

### Default seeded account

| Field | Value |
|--------|--------|
| Username | `admin` |
| Password | `Admin@123` |

Change this password in production and rotate `Jwt:Secret`.

## Migrations

Migrations live in `EmployeeManagement.Persistence`:

- **SQL Server:** `Migrations/` — `AppDbContext`
- **PostgreSQL:** `Migrations/Logging/` — `LoggingDbContext`

To add a new migration (examples):

```bash
dotnet ef migrations add <Name> --project EmployeeManagement.Persistence --startup-project EmployeeManagement.WebAPI --context AppDbContext

dotnet ef migrations add <Name> --project EmployeeManagement.Persistence --startup-project EmployeeManagement.WebAPI --context LoggingDbContext --output-dir Migrations/Logging
```

## API testing

### Standard response shape

Successful and error responses use `isSuccess`, `statusCode`, `message`, `data`, and `errors` (see assignment spec).

### Swagger

Open `/swagger`, use **Authorize** with `Bearer <token>` after login.

### cURL examples

**Login**

```bash
curl -k -X POST https://localhost:<port>/api/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"admin\",\"password\":\"Admin@123\"}"
```

**Authenticated request** (replace `TOKEN` and port):

```bash
curl -k https://localhost:<port>/api/employees -H "Authorization: Bearer TOKEN"
```

**Logout** (writes to PostgreSQL logs)

```bash
curl -k -X POST https://localhost:<port>/api/auth/logout -H "Authorization: Bearer TOKEN"
```

### Main endpoints (all under `/api` except Swagger)

| Area | Examples |
|------|-----------|
| Auth | `POST /auth/login`, `POST /auth/logout`, `POST /auth/register` |
| Employees | `GET/POST /employees`, `GET/PUT/DELETE /employees/{id}`, `PATCH /employees/{id}/deactivate` |
| Departments | `GET/POST /departments`, `GET/PUT/DELETE /departments/{id}` |
| Designations | `GET/POST /designations`, `GET/PUT/DELETE /designations/{id}` |
| Salaries | `GET /salaries/employee/{id}`, `POST /salaries/disburse`, `POST /salaries/adjustments`, … |
| Reports | `GET /reports/total-salary-disbursed/{year}/{month}`, `GET /reports/salary-distribution-by-department/{year}/{month}`, … |
| RBAC | `GET /roles` |
| Audit logs (PostgreSQL) | `GET /auditlogs?take=100` |

Authorization uses **permission claims** loaded from the database at login (`User → Role → Permission`), mapped to ASP.NET Core policies (not fixed role names on endpoints).

## Logging database

Structured audit rows (login, logout, entity changes where implemented, HTTP requests after the pipeline, unhandled errors) are stored in PostgreSQL in the `AuditLogs` table. Serilog continues to write to the console for local diagnostics.
