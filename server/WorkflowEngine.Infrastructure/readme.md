# 🏗️ WorkflowEngine.Infrastructure

The **WorkflowEngine.Infrastructure** project provides concrete, IO-bound implementations of the contracts defined in your Domain layer. It handles database persistence, HTTP calls, scheduling, and other external integrations.

---

## 📖 Table of Contents

- [Purpose](#purpose)
- [Key Components](#key-components)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Install Packages](#install-packages)
  - [Database Setup](#database-setup)
- [Entity Definitions](#entity-definitions)
- [Data Access Layer](#data-access-layer)
- [Dependency Injection](#dependency-injection)
- [Extending Infrastructure](#extending-infrastructure)
- [Dependencies](#dependencies)

---

## Purpose

- Fulfill Domain contracts with real-world systems (databases, HTTP clients, schedulers).
- Isolate external concerns from core business logic for easy testing and swapping implementations.
- Provide a shared persistence model for Users, Projects, and NodeInstances.

---

## Key Components

| Folder / File                          | Responsibility                                            |
| -------------------------------------- | --------------------------------------------------------- |
| `Entities/`                            | EF Core entity classes (`User`, `Project`, `NodeInstance`) |
| `Data/WorkflowEngineDbContext.cs`     | DbContext setup, table mappings, and schema configuration |
| `Migrations/` (generated)              | EF Core migration code to evolve the database schema      |
| *(Future)* `Schedulers/`, `Http/`, etc.| Implementations for Quartz.NET, HTTP node executor, etc. |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- **WorkflowEngine.Domain** project must be available in your solution
- EF Core CLI tool (optional, for migrations)
  ```bash
  dotnet tool install --global dotnet-ef --version 8.*
  ```

### Install Packages

Add the SQLite provider to this project:
```bash
cd src/WorkflowEngine.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.*
```

### Database Setup

1. **Configure connection string** in your API project `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=Data/workflow.db"
   }
   ```
2. **Scaffold the initial migration** (from the solution root):
   ```bash
   dotnet ef migrations add InitialCreate \
     --project WorkflowEngine.Infrastructure \
     --startup-project WorkflowEngine.Api
   ```
3. **Apply the migration** to create `workflow.db` in your API output:
   ```bash
   dotnet ef database update \
     --project WorkflowEngine.Infrastructure \
     --startup-project WorkflowEngine.Api
   ```

> After this step, your SQLite file will contain the `Users`, `Projects`, and `NodeInstances` tables.

---

## Entity Definitions

- **User** (`Entities/User.cs`): stores authenticated Windows usernames and optional password hashes.
- **Project** (`Entities/Project.cs`): represents a workflow project owned by a User.
- **NodeInstance** (`Entities/NodeInstance.cs`): persists each node’s type, configuration JSON, and canvas position.

These entities are configured in `WorkflowEngineDbContext.OnModelCreating` to enforce PKs, FKs, and cascading behaviors.

---

## Data Access Layer

The `WorkflowEngineDbContext` exposes:
```csharp
public DbSet<User> Users { get; set; }
public DbSet<Project> Projects { get; set; }
public DbSet<NodeInstance> NodeInstances { get; set; }
```
Use standard EF Core patterns (LINQ, async) from services or controllers to query and update data.

---

## Dependency Injection

Register the persistence layer in your API startup:

```csharp
// WorkflowEngine.Api/Configuration/ServiceCollectionExtensions.cs
public static IServiceCollection AddPersistence(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddDbContext<WorkflowEngineDbContext>(opts =>
        opts.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    
    // Future: register repository interfaces here

    return services;
}
```
Then in `Program.cs`:
```csharp
builder.Services.AddPersistence(builder.Configuration);
```

---

## Extending Infrastructure

- **Schedulers**: add a `QuartzScheduler` implementing `IScheduler` from Domain.
- **HTTP Executors**: wrap `HttpClientFactory` for `HttpRequest` node execution.
- **Validation**: integrate JSON Schema validation for node configuration.
- **Alternate Stores**: swap SQLite for SQL Server or Postgres by changing provider and connection string.

---

## Dependencies

- **WorkflowEngine.Domain**: domain models and interfaces
- **Microsoft.EntityFrameworkCore.Sqlite**: SQLite EF Core provider

*(Optional for future features)*
- **Microsoft.EntityFrameworkCore.Design**: design-time migrations support
- **Quartz**: job scheduler implementation
- **System.Net.Http**: HTTP client factory integration

---

*This README serves as a guide to understanding and extending the Infrastructure layer of your WorkflowEngine solution.*