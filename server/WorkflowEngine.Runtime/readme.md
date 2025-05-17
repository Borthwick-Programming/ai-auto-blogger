## ⚙️ WorkflowEngine.Runtime

The `WorkflowEngine.Runtime` project contains the **core orchestration logic** for registering and managing node definitions.  
It acts as the engine that knows how nodes work together — but not how they're persisted or exposed.

### ✅ Purpose

- Implements `INodeRegistry` to manage available `NodeDefinition`s
- Serves as the **runtime logic layer** (execution coordination will live here)
- Bridges the API layer and the domain model

---

### 📦 Contents

| File | Description |
|------|-------------|
| `Interfaces/INodeRegistry.cs` | Contract for managing registered nodes |
| `Services/InMemoryNodeRegistry.cs` | In-memory implementation used at startup for testing and bootstrapping |

---

### 🔧 Future Responsibilities

- Manage node graph execution (`WorkflowExecutor`)
- Trigger nodes in response to inputs, events, or schedules
- Maintain runtime session state

---

### 🧼 Dependencies

- WorkflowEngine.Domain
