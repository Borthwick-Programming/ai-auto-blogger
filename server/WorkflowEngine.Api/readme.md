# 🌐 WorkflowEngine.Api

The **WorkflowEngine.Api** project serves as the **HTTP entry point** for the Workflow Engine. It exposes RESTful endpoints for managing workflow **projects** and **nodes**, and integrates with Swagger for interactive API exploration.

---

## 📖 Table of Contents

- [Purpose](#purpose)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Clone & Build](#clone--build)
  - [Configure Database](#configure-database)
  - [Run Migrations](#run-migrations)
  - [Launch API](#launch-api)
- [Dependency Injection](#dependency-injection)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
  - [Node Definitions](#node-definitions)
  - [Projects](#projects)
  - [Node Instances](#node-instances)
- [Swagger / OpenAPI](#swagger--openapi)
- [Extending the API](#extending-the-api)
- [Dependencies](#dependencies)

---

## Purpose

- Provide a RESTful surface for creating, listing, and deleting **Projects**.
- Persist user data and workflow graphs via **SQLite** and **EF Core**.
- Offer **NodeDefinitions** for building workflow canvases in a UI.
- Serve as the host for **Windows-integrated authentication** and future JWT support.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- **WorkflowEngine.Core**, **WorkflowEngine.Domain**, **WorkflowEngine.Infrastructure**, **WorkflowEngine.Runtime** projects present
- **EF Core CLI** (optional, for migrations)
  ```bash
  dotnet tool install --global dotnet-ef --version 8.*
  ```

---

## Getting Started

### Clone & Build

```bash
git clone <repo-url>
cd repo/dev/ai-auto-blogger/server/WorkflowEngine.Api
dotnet build
```

### Configure Database

1. **Data folder**: Ensure `WorkflowEngine.Api/Data` exists.  
2. **Connection string**: In `appsettings.json` set:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=Data/workflow.db"
   }
   ```
3. **Copy settings**: In `WorkflowEngine.Api.csproj`, include:
   ```xml
   <ItemGroup>
     <None Include="Data\**\*.*">
       <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
     </None>
   </ItemGroup>
   ```

### Run Migrations

From the solution root:
```bash
# Scaffold EF migration
dotnet ef migrations add InitialCreate \
  --project WorkflowEngine.Infrastructure \
  --startup-project WorkflowEngine.Api

# Apply migration and create SQLite DB
dotnet ef database update \
  --project WorkflowEngine.Infrastructure \
  --startup-project WorkflowEngine.Api
```  
Or use the Package Manager Console in Visual Studio:
```powershell
# Default project: WorkflowEngine.Infrastructure
Add-Migration InitialCreate -StartupProject WorkflowEngine.Api
Update-Database -StartupProject WorkflowEngine.Api
```

### Launch API

```bash
dotnet run --project WorkflowEngine.Api
dopen http://localhost:5015/swagger
```

---

## Dependency Injection

In `Program.cs` (minimal hosting):

```csharp
var builder = WebApplication.CreateBuilder(args);

// Persistence, Auth, Core services
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## Authentication

- **Windows Integrated** via Negotiate (IIS Express): secure endpoints with `[Authorize]`.  
- Future support for JWT can be toggled in `AddAuthenticationServices`.

---

## API Endpoints

### Node Definitions

| Method | Path         | Description                            |
|--------|--------------|----------------------------------------|
| GET    | `/api/nodes` | List all available node definitions.   |

### Projects

| Method | Path           | Body                             | Description                       |
|--------|----------------|----------------------------------|-----------------------------------|
| GET    | `/api/projects`| —                                | List projects for current user.   |
| POST   | `/api/projects`| `{ name: string }`               | Create a new project.             |
| GET    | `/api/projects/{id}` | —                      | Get project details by ID.        |
| DELETE | `/api/projects/{id}` | —                      | Delete a project by ID.           |

### Node Instances

| Method | Path                                         | Body                                         | Description                                 |
|--------|----------------------------------------------|----------------------------------------------|---------------------------------------------|
| GET    | `/api/projects/{projectId}/nodeinstances`    | —                                            | List nodes in a project.                   |
| GET    | `/api/projects/{projectId}/nodeinstances/{id}` | —                                         | Get a specific node instance.              |
| POST   | `/api/projects/{projectId}/nodeinstances`    | `{ nodeTypeId, configurationJson, x, y }`    | Create a new node instance.                |
| PUT    | `/api/projects/{projectId}/nodeinstances/{id}` | `{ id, nodeTypeId, configurationJson, x, y}` | Update node configuration/position.        |
| DELETE | `/api/projects/{projectId}/nodeinstances/{id}`| —                                            | Remove a node from the project.            |

---

## Swagger / OpenAPI

- Swagger UI available at `/swagger` for interactive testing.  
- Automatically generated schemas for request and response models.

---

## Extending the API

- **Add new endpoints** for node connections (wiring), execution triggers, or bulk operations.  
- **Integrate React UI** to consume these endpoints and render a workflow canvas.  
- **Swap authentication** from Negotiate to JWT by adjusting `AddAuthenticationServices`.

---

## Dependencies

- **WorkflowEngine.Core**, **Domain**, **Infrastructure**, **Runtime** projects  
- **Microsoft.AspNetCore.Authentication.Negotiate**  
- **Swashbuckle.AspNetCore** (Swagger)  
- **Microsoft.EntityFrameworkCore.Design** (migrations)  
- **Microsoft.EntityFrameworkCore.Tools** (PMC)

---

*This README provides guidance for running, configuring, and extending the API layer of your WorkflowEngine solution.*