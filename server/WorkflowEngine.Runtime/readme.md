# ⚙️ WorkflowEngine.Runtime

The **WorkflowEngine.Runtime** project contains the core execution engine for your workflows. It provides an in-memory registry of node definitions, the skeleton for orchestrating node graph execution, and a foundation for runtime session management.

---

## 📖 Table of Contents

- [Prerequisites](#prerequisites)
- [Build & Run](#build--run)
- [Key Components](#key-components)
- [Registering Nodes](#registering-nodes)
- [Bootstrapping the Engine](#bootstrapping-the-engine)
- [Extending the Runtime](#extending-the-runtime)
- [Dependencies](#dependencies)

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- **WorkflowEngine.Domain** project (models for node definitions, ports, and execution triggers)

---

## Build & Run

The runtime can be built and executed as a standalone console app or hosted inside a Windows service:

```bash
cd src/WorkflowEngine.Runtime
dotnet build
dotnet run --project WorkflowEngine.Runtime
```

> ⚠️ By default there is no JSON loader in this project alone—you must register definitions programmatically or load them via an external host (e.g. your API).

---

## Key Components

### Interfaces

- `INodeRegistry`  
  Contract for registering and retrieving available `NodeDefinition` instances.

### Services

- `InMemoryNodeRegistry`  
  Default in-memory implementation. Holds node definitions in a simple list and provides lookup by ID.

---

## Registering Nodes

To populate the registry, call:

```csharp
var registry = new InMemoryNodeRegistry();
registry.Register(myExtendedNodeDefinition);
```

Where `myExtendedNodeDefinition` is an instance of `ExtendedNodeDefinition` from **WorkflowEngine.Domain**.

Custom registry implementations can replace `InMemoryNodeRegistry` by implementing `INodeRegistry` and registering definitions from any source (database, file system, remote API).

---

## Bootstrapping the Engine

While the full `WorkflowExecutor` is a future addition, you can begin wiring up your execution logic by:

```csharp
using WorkflowEngine.Runtime.Interfaces;
using WorkflowEngine.Runtime.Services;

// 1. Create and populate your registry
var registry = new InMemoryNodeRegistry();
registry.Register(...);

// 2. (Placeholder) Instantiate your executor
// var executor = new WorkflowExecutor(registry);
// executor.Run(myProjectGraph);
```

> ✳️ **Coming Soon**: A `WorkflowExecutor` service that traverses node graphs, triggers conditional nodes, and flows data between node inputs/outputs.

---

## Extending the Runtime

Consider adding:

- **Persistent Sessions**: log node execution results to a database for replay or debugging.
- **Custom Triggers**: time-based or external-event triggers via schedulers.
- **Distributed Execution**: partition workloads across multiple worker processes.

---

## Dependencies

- **WorkflowEngine.Domain**: shared models for definitions and execution metadata.
- **Microsoft.Extensions.Hosting** (future): for building a hosted service.
- **Microsoft.Data.Sqlite** (future): local storage of runtime state or logs.
