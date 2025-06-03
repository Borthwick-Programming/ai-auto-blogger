# 🚀 WorkflowEngine.Core

The **WorkflowEngine.Core** project implements the **application/business logic layer** of the Workflow Engine. It orchestrates use-cases defined by your domain, interfaces with the persistence layer (Infrastructure), and exposes clean contracts for the API layer to consume.

---

## 📖 Table of Contents

- [Purpose](#purpose)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Key Components](#key-components)
  - [Interfaces](#interfaces)
  - [Models (DTOs & Requests)](#models-dtos--requests)
  - [Services (Implementations)](#services-implementations)
- [Dependency Injection](#dependency-injection)
- [Usage](#usage)
- [Extending Core](#extending-core)
- [Dependencies](#dependencies)

---

## Purpose

- **Orchestrate business logic**: Coordinates domain models and persistence calls to fulfill user actions.
- **Define use-case contracts**: Exposes clear interfaces (`IProjectService`, `INodeInstanceService`) that can be consumed by any host (API, console, tests).
- **Maintain separation of concerns**: Keeps controllers thin and Infrastructure focused purely on data I/O.

---

## Project Structure

```plaintext
WorkflowEngine.Core/
├─ Interfaces/
│   ├─ IProjectService.cs
│   └─ INodeInstanceService.cs
├─ Models/
│   ├─ ProjectDto.cs
│   ├─ CreateProjectRequest.cs
│   ├─ NodeInstanceDto.cs
│   ├─ CreateNodeInstanceRequest.cs
│   └─ UpdateNodeInstanceRequest.cs
├─ Services/
│   ├─ ProjectService.cs
│   └─ NodeInstanceService.cs
└─ readme.md
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- **WorkflowEngine.Domain** (domain contracts and models)
- **WorkflowEngine.Infrastructure** (EF Core DbContext and entities)

### Install

Make sure your **Core** project is referenced by the **API** project:
```bash
cd src/WorkflowEngine.Api
dotnet add reference ../WorkflowEngine.Core/WorkflowEngine.Core.csproj
```

---

## Key Components

### Interfaces

- **IProjectService**  
  Defines use-cases for project management (list, get, create, delete).  
  ```csharp
  Task<IEnumerable<ProjectDto>> ListAsync(string windowsUser);
  Task<ProjectDto?> GetAsync(Guid id, string windowsUser);
  Task<ProjectDto> CreateAsync(string windowsUser, CreateProjectRequest req);
  Task<bool> DeleteAsync(Guid id, string windowsUser);
  ```

- **INodeInstanceService**  
  Defines CRUD and wiring for node instances within a project.  
  ```csharp
  Task<IEnumerable<NodeInstanceDto>> ListAsync(Guid projectId, string windowsUser);
  Task<NodeInstanceDto?> GetAsync(Guid projectId, Guid nodeId, string windowsUser);
  Task<NodeInstanceDto> CreateAsync(...);
  Task<bool> UpdateAsync(...);
  Task<bool> DeleteAsync(...);
  ```

### Models (DTOs & Requests)

Holds data-transfer objects and request shapes used by services and controllers:

- **ProjectDto**: `{ Guid Id, string Name }`  
- **CreateProjectRequest**: `{ string Name }`  
- **NodeInstanceDto**: `{ Guid Id, Guid ProjectId, string NodeTypeId, string ConfigurationJson, double PositionX, double PositionY }`  
- **CreateNodeInstanceRequest**: same as `NodeInstanceDto` without `Id`  
- **UpdateNodeInstanceRequest**: includes `Guid Id` plus other fields  

### Services (Implementations)

- **ProjectService**  
  - Uses `WorkflowEngineDbContext` to load/create/delete projects.  
  - Auto-provisions a `User` record for Windows-integrated auth.  
  - Ensures only the owner can manage their projects.

- **NodeInstanceService**  
  - Depends on `ProjectService` to verify project ownership.  
  - Performs CRUD on `NodeInstance` entities.  
  - Maps EF entities to/from `NodeInstanceDto` and request models.

Both implementations live in `Services/` and reference the Infrastructure layer for data access.

---

## Dependency Injection

Register Core services in your API startup:

```csharp
// WorkflowEngine.Api/Configuration/ServiceCollectionExtensions.cs
using WorkflowEngine.Core.Interfaces;
using WorkflowEngine.Core.Services;

public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<INodeInstanceService, NodeInstanceService>();
    return services;
}
```

Then call in **Program.cs**:
```csharp
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddApplicationServices();
```

---

## Usage

With DI configured, controllers can inject `IProjectService` and `INodeInstanceService` to handle HTTP requests. Core services perform all business logic, leaving controllers free of data-access code.

Example in a controller:
```csharp
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projects;
    public ProjectsController(IProjectService projects) => _projects = projects;

    [HttpGet]
    public Task<IActionResult> GetAll() =>
        _projects.ListAsync(User.Identity.Name!)
                 .ContinueWith(t => Ok(t.Result));
}
```

---

## Extending Core

- **Add new use-cases**: e.g. `IConnectionService` for node wiring operations.
- **Introduce validation**: throw domain-specific exceptions when requests violate business rules.
- **Unit test**: mock `WorkflowEngineDbContext` or use an in-memory provider (e.g. SQLite in-memory) to verify service behavior.

---

## Dependencies

- **WorkflowEngine.Domain**: domain models and interfaces
- **WorkflowEngine.Infrastructure**: EF Core DbContext and entities

No external NuGet packages are required in this project; it targets **net8.0** by default.