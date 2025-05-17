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
