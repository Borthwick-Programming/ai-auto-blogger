# 🧠 WorkflowEngine.Domain

The **WorkflowEngine.Domain** project defines the **fundamental business concepts** of the workflow engine. This layer is **framework‑agnostic** and contains only pure C# types and interfaces that represent the core domain model.

---

## 📖 Table of Contents

- [Purpose](#purpose)
- [Key Concepts](#key-concepts)
- [Project Structure](#project-structure)
- [Domain Models](#domain-models)
- [Value Objects](#value-objects)
- [Interfaces & Contracts](#interfaces--contracts)
- [Validation & Invariants](#validation--invariants)
- [Extending the Domain](#extending-the-domain)
- [Dependencies](#dependencies)

---

## Purpose

- **Encapsulate** the rules, data structures, and behaviors that define a workflow and its components.
- **Isolate** core business logic from infrastructure, UI, and runtime concerns.
- **Promote** testability and reuse by avoiding external dependencies.

---

## Key Concepts

- **NodeDefinition**: Describes a type of node (its configuration schema, input/output ports, and metadata).
- **PortDefinition**: Defines individual input or output ports, including data types and optional default values.
- **ExecutionTrigger**: Specifies when a node should be executed (e.g., manually, on schedule, on event).
- **NodeId**: A strongly‑typed wrapper for node identifiers, ensuring compile‑time safety.
- **IScheduler**: Contract for pluggable scheduling mechanisms (e.g., cron, timer, external triggers).

---

## Project Structure

```
WorkflowEngine.Domain/
├─ Interfaces/           # Pluggable contracts (e.g., IScheduler)
│   └─ IScheduler.cs
├─ Models/               # Core domain entities and definitions
│   ├─ NodeDefinition.cs
│   ├─ PortDefinition.cs
│   └─ ExecutionTrigger.cs
├─ ValueObjects/         # Strongly‑typed value objects and identifiers
│   └─ NodeId.cs
└─ WorkflowEngine.Domain.csproj
```

---

## Domain Models

### `NodeDefinition`
- **Properties**:
  - `NodeId Id`: Unique identifier for the node type.
  - `string Name`: Human‑readable name.
  - `string Description`: Optional description text.
  - `IEnumerable<PortDefinition> Inputs`: Input port definitions.
  - `IEnumerable<PortDefinition> Outputs`: Output port definitions.
  - `ExecutionTrigger Trigger`: How and when to execute.
  - `object? VisualMetadata`: Optional UI metadata.

### `PortDefinition`
- **Properties**:
  - `string Name`: Port name.
  - `Type DataType`: CLR type of the data.
  - `bool IsRequired`: If true, caller must supply a value.
  - `object? DefaultValue`: Default value when not provided.

### `ExecutionTrigger`
- **Enum Values**:
  - `Manual`: Triggered explicitly by user action.
  - `OnSchedule`: Runs on a cron or timer schedule.
  - `OnEvent`: Runs when an external event occurs.

---

## Value Objects

### `NodeId`
A simple wrapper around `Guid`:
```csharp
public readonly record struct NodeId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
```
Use `NodeId` instead of raw `Guid` to prevent mixing up identifiers in code.

---

## Interfaces & Contracts

### `IScheduler`
Defines a pluggable scheduling mechanism for nodes:
```csharp
public interface IScheduler
{
    /// <summary>
    /// Schedule a recurring job based on a cron expression or interval.
    /// </summary>
    void Schedule(NodeId nodeId, ExecutionTrigger trigger, Func<Task> execute);
}
```
Implement `IScheduler` in the Infrastructure layer (e.g., with Quartz.NET) to drive `OnSchedule` triggers.

---

## Validation & Invariants

The Domain layer may enforce the following invariants:

- A `NodeDefinition` must have at least one output port.
- Required ports (`IsRequired == true`) must not have a null default value.
- `ExecutionTrigger` values must match the node’s capabilities (e.g., only event‑driven nodes can use `OnEvent`).

Consider adding guard clauses or factory methods in this project to validate these invariants at construction time.

---

## Extending the Domain

- Add new `ExecutionTrigger` types (e.g., `OnWebhook`, `OnFileChanged`).
- Create subtypes of `NodeDefinition` for specialized behavior.
- Introduce domain events (`NodeCreated`, `ProjectSaved`) and an `IDomainEventDispatcher` interface.

Keep all extensions free of external dependencies to maintain the purity of the Domain layer.

---

## Dependencies

- **None**! This project targets **.NET 8** and references no external NuGet packages.

All other projects in the solution depend on `WorkflowEngine.Domain` for the canonical business model.
