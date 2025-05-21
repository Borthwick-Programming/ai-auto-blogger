## 🌐 WorkflowEngine.Api

The `WorkflowEngine.Api` project is the **entry point for the workflow engine's HTTP interface**, built using ASP.NET Core Web API.

### ✅ Purpose

- Exposes endpoints to interact with the workflow engine (e.g., list available nodes)
- Hosts Swagger for API documentation and testing
- Acts as the presentation layer in clean architecture

---

### 📦 Contents

| File | Description |
|------|-------------|
| `Program.cs` | Application entry point and DI setup (minimal hosting) |
| `Controllers/NodeDefinitionsController.cs` | Provides a GET `/api/nodes` endpoint to fetch all registered node definitions |

---

### 🔧 Key Features

- **Swagger** UI via Swashbuckle
- **Dependency Injection** wired for core services (`INodeRegistry`)
- Designed to be thin — delegates logic to `Runtime` and uses contracts from `Domain`

---

### 🧼 Dependencies

- ASP.NET Core
- Swashbuckle.AspNetCore (Swagger/OpenAPI generation)
- WorkflowEngine.Domain
- WorkflowEngine.Runtime
- WorkflowEngine.Infrastructure

### For SQL Lite install
- we have a Data folder. We need to make sure that the following NuGet packages are referenced:
-- dotnet add WorkflowEngine.Api package Microsoft.EntityFrameworkCore.Design
-- dotnet add WorkflowEngine.Api package Microsoft.EntityFrameworkCore.Tools

### Using the .NET CLI
-From solution root (where WorkflowEngine.sln lives), run:

--Scaffold the initial migration
`bash
Copy
Edit
dotnet ef migrations add InitialCreate \
  --project WorkflowEngine.Infrastructure \
  --startup-project WorkflowEngine.Api
Apply the migration and create the database

bash
Copy
Edit
dotnet ef database update \
  --project WorkflowEngine.Infrastructure \
  --startup-project WorkflowEngine.Api`

  ### Using Visual Studio Package Manager Console
- Set Default project to WorkflowEngine.Infrastructure in the PMC toolbar.
- (One-time only) Add the EF Tools to the API project:
	`dotnet add WorkflowEngine.Api package Microsoft.EntityFrameworkCore.Tools`
- Scaffold the migration:
	`Add-Migration InitialCreate 
  -Project WorkflowEngine.Infrastructure 
  -StartupProject WorkflowEngine.Api`
  -Apply the migration:
  `Update-Database 
  -Project WorkflowEngine.Infrastructure 
  -StartupProject WorkflowEngine.Api`