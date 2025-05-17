## 🧠 WorkflowEngine.Domain

The `WorkflowEngine.Domain` project defines the **core domain model** of the workflow engine.  
This layer is **independent of frameworks**, databases, APIs, and runtime behavior — it expresses only the **essential business concepts**.

### ✅ Purpose

- Define what a **Node** is, including configuration schema, inputs/outputs, and metadata
- Describe **execution triggers** (when nodes should run)
- Provide clean, **framework-agnostic interfaces** (e.g., `IScheduler`)
- Enable **strong typing** through value objects (e.g., `NodeId`)
- Ensure the **core business logic is portable and testable**

---

### 📦 Contents

| File | Description |
|------|-------------|
| `Models/NodeDefinition.cs` | Describes the structure of a node (name, type, config schema, inputs/outputs) |
| `Models/PortDefinition.cs` | Describes a node's input or output data port |
| `Models/ExecutionTrigger.cs` | Enum for defining node scheduling and event behavior |
| `Interfaces/IScheduler.cs` | Contract for pluggable scheduling mechanisms |
| `ValueObjects/NodeId.cs` | Optional strongly-typed wrapper for node identifiers |

---

### 🔄 Why Keep This Separate?

- Keeps business logic clean, testable, and implementation-agnostic
- Other layers like `Api`, `Runtime`, or `Infrastructure` **depend on this**, but it depends on **nothing**
- Enables future scenarios like
