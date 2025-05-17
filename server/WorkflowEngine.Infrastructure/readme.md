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
