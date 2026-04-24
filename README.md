# Job Application Tracker API

A RESTful API for tracking job applications, built with .NET 8, Entity Framework Core, and PostgreSQL. Designed with a clean layered architecture and production-aware patterns.

## Tech Stack

- **Framework:** .NET 8 / ASP.NET Core
- **Language:** C#
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL
- **Containerisation:** Docker
- **API Docs:** Swagger / OpenAPI

## Architecture

The project follows a strict layered architecture with clear separation of concerns:

```
Backend.API           → Controllers, Middleware
Backend.Core          → Models, DTOs, Interfaces, Exceptions
Backend.Infrastructure → DbContext, Repositories, Services
```

**Request flow:**
```
Controller → Service → Repository → DbContext → PostgreSQL
```

- Controllers are thin — they receive requests, call the service, and return responses
- Services own all business logic and DTO mapping
- Repositories handle all data access
- Entities never cross the service boundary outward — only DTOs are exposed

## Features

- **Full CRUD** — Create, read, update, and delete job applications
- **Pagination** — Page through results with configurable page size (max 50)
- **Sorting** — Sort by `dateApplied`, `company`, `role`, or `status` in either direction
- **Filtering** — Filter by status (exact) or company (partial, case-insensitive)
- **DTO Layer** — Entities are never exposed directly in API responses
- **Global Exception Handling** — Consistent JSON error responses with trace IDs
- **Docker** — PostgreSQL runs in a container, no local install required

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run Locally

**1. Clone the repository**
```bash
git clone git@github.com:a-elhussein/JobTracker.git
cd JobTracker
```

**2. Start PostgreSQL with Docker**
```bash
docker run --name jobtracker-db \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=jobtracker \
  -p 5432:5432 \
  -d postgres
```

**3. Apply database migrations**
```bash
cd Backend/Backend.API
dotnet ef database update
```

**4. Run the API**
```bash
dotnet run
```

**5. Open Swagger**
```
https://localhost:{port}/swagger
```

## API Endpoints

### Get All Job Applications

```
GET /api/JobApplications
```

**Query Parameters:**

| Parameter  | Type    | Default      | Description                                      |
|------------|---------|--------------|--------------------------------------------------|
| `page`     | int     | 1            | Page number                                      |
| `pageSize` | int     | 10           | Results per page (max 50)                        |
| `sortBy`   | string  | dateApplied  | Field to sort by: `dateApplied`, `company`, `role`, `status` |
| `descending` | bool  | true         | Sort direction                                   |
| `status`   | string  | —            | Filter by status (exact, case-insensitive)       |
| `company`  | string  | —            | Filter by company name (partial, case-insensitive) |

**Example:**
```
GET /api/JobApplications?page=1&pageSize=10&sortBy=dateApplied&descending=true&status=interview
```

**Response:**
```json
{
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "company": "Google",
      "role": "Backend Engineer",
      "status": "Interview",
      "dateApplied": "2026-04-20T10:00:00Z",
      "notes": "Phone screen scheduled",
      "salaryRange": "£70,000 - £90,000",
      "source": "LinkedIn"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

---

### Get Job Application by ID

```
GET /api/JobApplications/{id}
```

**Response:** `200 OK` with job application object, or `404 Not Found`

---

### Create Job Application

```
POST /api/JobApplications
```

**Request Body:**
```json
{
  "company": "Google",
  "role": "Backend Engineer",
  "status": "Applied",
  "dateApplied": "2026-04-20T10:00:00Z",
  "notes": "Referral from a contact",
  "salaryRange": "£70,000 - £90,000",
  "source": "LinkedIn"
}
```

**Response:** `201 Created` with the created resource

---

### Update Job Application

```
PUT /api/JobApplications/{id}
```

**Request Body:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "company": "Google",
  "role": "Backend Engineer",
  "status": "Interview",
  "dateApplied": "2026-04-20T10:00:00Z",
  "notes": "Phone screen passed",
  "salaryRange": "£70,000 - £90,000",
  "source": "LinkedIn"
}
```

**Response:** `200 OK` with updated resource, or `404 Not Found`

---

### Delete Job Application

```
DELETE /api/JobApplications/{id}
```

**Response:** `204 No Content`, or `404 Not Found`

---

## Error Responses

All errors return a consistent JSON envelope:

```json
{
  "statusCode": 404,
  "message": "JobApplication with id '3fa85f64-5717-4562-b3fc-2c963f66afa6' was not found.",
  "errors": null,
  "traceId": "0HNL1RSK29JIH:00000003"
}
```

| Status | Cause |
|--------|-------|
| `400`  | Invalid request body or query parameters |
| `404`  | Resource not found |
| `500`  | Unexpected server error |

## Roadmap

- [ ] JWT Authentication
- [ ] Analytics endpoints (applications per week, status breakdown)
- [ ] CI/CD pipeline with GitHub Actions
