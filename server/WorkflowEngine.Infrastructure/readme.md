## 🏗️ WorkflowEngine.Infrastructure

The `WorkflowEngine.Infrastructure` project contains **external implementations** that fulfill domain contracts using real-world systems.

### ✅ Purpose

- Houses IO-bound code (databases, HTTP clients, file systems, schedulers)
- Implements interfaces defined in `Domain` (e.g., `IScheduler`)
- Can be swapped, mocked, or extended independently of core logic

---

### 📦 Contents

| File | Description |
|------|-------------|
| *Coming Soon* | Will include implementations like `QuartzScheduler`, `HttpRequestExecutor`, etc. |

---

### 🛠 Examples of What Will Live Here

- A scheduler using **Quartz.NET**
- An HTTP client wrapper for executing `HttpRequest` nodes
- A JSON schema validator for config validation
- A storage adapter for workflow definitions

---

### 🧼 Dependencies

- WorkflowEngine.Domain
- Optional: external libraries like Quartz, HttpClientFactory, or Entity Framework

## Database Setup

We use EF Core with SQLite for our local persistence. Follow the steps below to scaffold the initial migration and create the `workflow.db` file.
Ensure the following NuGet packages are referenced:
dotnet add WorkflowEngine.Infrastructure package Microsoft.EntityFrameworkCore.Sqlite

### Prerequisites

- Install the EF Core CLI tool globally (if you haven’t already):
  ```bash
  dotnet tool install --global dotnet-ef --version 9.*

