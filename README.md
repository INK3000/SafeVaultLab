# SafeVault Security Coursework Project

SafeVault is a course project that demonstrates secure backend development in ASP.NET Core with Microsoft Copilot-assisted implementation.

This submission combines work from previous activities into one project:

1. Secure input validation and SQL injection prevention.
2. Authentication and authorization with role-based access control (RBAC).
3. Debugging and fixing security vulnerabilities (including SQL injection and XSS scenarios).

## Tech Stack

- .NET 10 (SDK 10.x, target framework `net10.0`)
- ASP.NET Core Web API
- Cookie Authentication
- NUnit

## Project Structure

```text
SafeVault/
├── Controllers/
├── DTOs/
├── Data/
├── Models/
├── Security/
├── Services/
├── Program.cs
├── SafeVault.csproj
├── Directory.Build.props
├── database.sql
├── webform.html
└── README.md

SafeVault.Tests/
└── SafeVault.Tests.csproj
```

## What Is Implemented

- Secure input handling in `Security/InputSanitizer.cs`.
- Password hashing and verification in `Security/PasswordService.cs`.
- Authentication flows (`register`, `login`, `logout`) in `Controllers/AuthController.cs`.
- RBAC-protected endpoints in `Controllers/SecureController.cs`.
- Security-related test coverage in `SafeVault.Tests/*.cs`.

## Quick Start (.NET 10)

From the repository root:

```bash
dotnet --info
dotnet restore
dotnet build
dotnet run --project SafeVault.csproj
```

In a second terminal, run tests:

```bash
dotnet test SafeVault.Tests/SafeVault.Tests.csproj
```

## API Endpoints

Base URL (default): `http://localhost:5000` or `https://localhost:5001` (depending on launch profile/environment).

- `GET /` - Health endpoint (`SafeVault is running.`)
- `POST /api/auth/register` - Register a user (`username`, `password`, `role`)
- `POST /api/auth/login` - Authenticate and create cookie session
- `POST /api/auth/logout` - Clear authentication cookie
- `GET /api/secure/profile` - Any authenticated user
- `GET /api/secure/user-area` - Role: `user` or `admin`
- `GET /api/secure/admin-dashboard` - Role: `admin`
- `GET /access-denied` - Returns `403`

## How To Verify the Project Manually

You can verify with Postman or curl.

1. Register users with different roles (`user`, `admin`).
2. Log in and keep session cookies.
3. Call protected endpoints and confirm RBAC behavior.

Example requests:

```bash
# Register normal user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"alice","password":"Password123!","role":"user"}'

# Register admin
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"admin1","password":"AdminPass123!","role":"admin"}'

# Login as user and store cookie
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{"username":"alice","password":"Password123!"}'

# Access allowed endpoint for user
curl http://localhost:5000/api/secure/user-area -b cookies.txt

# Access admin-only endpoint as user (should be denied)
curl -i http://localhost:5000/api/secure/admin-dashboard -b cookies.txt
```

## About `webform.html` and `database.sql`

- `webform.html` is a standalone HTML form sample from earlier activity context.
- It is not wired into the current API pipeline, and `/submit` is not implemented in `Program.cs`.
- `database.sql` is an illustrative SQL script and not used by the in-memory repository at runtime.

For grading and technical verification, focus on the API + tests.

## Security Work Included in This Submission

### Input Validation and Injection Prevention

- Username validation via strict regex and trimming.
- Email validation with format checks.
- Rejection of malicious payloads in sanitizer methods.
- Documentation and examples of parameterized SQL patterns for SQL injection prevention.

### Authentication and Authorization (RBAC)

- Cookie-based authentication with ASP.NET Core auth middleware.
- Role claim assignment at login.
- Role-based endpoint protection with `[Authorize]` and role constraints.

### Vulnerability Debugging and Fixes

- SQL injection risk addressed by documenting and applying parameterized-query patterns.
- XSS risk addressed by output encoding (`HtmlEncode`) and input sanitization tests.
- Weak input handling addressed with explicit validation and defensive exceptions.

## Test Coverage

Security tests in `SafeVault.Tests` cover:

- SQL injection-like username payload rejection.
- XSS payload handling and HTML encoding behavior.
- Registration/login success and failure paths.
- Duplicate-user rejection.
- Authorization/role checks.

Run:

```bash
dotnet test SafeVault.Tests/SafeVault.Tests.csproj
```

## Microsoft Copilot Usage (Course Requirement)

This coursework is intentionally focused on learning secure coding with Microsoft Copilot. Copilot was used to accelerate implementation and debugging, while the final behavior was validated manually and with tests.

Copilot-assisted areas:

- Generating secure input validation and sanitizer scaffolding.
- Suggesting SQL injection-safe coding patterns (parameterized queries).
- Implementing authentication and authorization patterns, including RBAC endpoint guards.
- Generating and refining test cases for security and auth scenarios.
- Supporting iterative debugging of vulnerabilities and code correctness.

## Assignment Rubric Alignment (30 points)

This project addresses all required grading items:

1. (5 pts) GitHub repository created for submission.
2. (5 pts) Copilot used to generate secure code for input validation and SQL injection prevention.
3. (5 pts) Copilot used to implement authentication and authorization, including RBAC.
4. (5 pts) Security vulnerabilities (including SQL injection and XSS) were debugged and resolved.
5. (5 pts) Security-focused tests were generated and executed.
6. (5 pts) This README includes a concise summary of vulnerabilities, fixes, and Copilot's role in debugging.

## Notes

- The current app uses an in-memory repository (`Data/UserRepository.cs`) for coursework simplicity.
- In production, this would be replaced with persistent storage, stricter transport/security settings, CSRF protections, lockout policies, and audit logging.
