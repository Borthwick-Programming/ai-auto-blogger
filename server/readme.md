# 🔗 WorkflowEngine Solution

A **Make.com–style** visual workflow automation engine for chaining together nodes to automate tasks such as WordPress blog posts, video generation, AI calls, and more.

---

## 📖 Table of Contents

- [Overview](#overview)  
- [Architecture & Projects](#architecture--projects)  
- [Getting Started](#getting-started)  
- [Prerequisites](#prerequisites)  
- [Build & Run](#build--run)  
- [Project READMEs](#project-readmes)  
- [Next Steps](#next-steps)

---

## Overview

This solution provides a **full-stack** workflow engine with:  

1. **Domain**: Core business concepts (nodes, ports, triggers).  
2. **Infrastructure**: EF Core + SQLite persistence & future integrations.  
3. **Core**: Application logic (project/node‑instance services).  
4. **API**: REST surface with Windows‑integrated auth and Swagger UI.  
5. **Runtime**: In‑memory registry & execution engine scaffold.

You’ll be able to visually compose workflows in a React frontend, save them to SQLite, and execute them via a .NET host or Windows service.

---

## Architecture & Projects

```
WorkflowEngine.sln
├─ WorkflowEngine.Domain       # Domain models and contracts
├─ WorkflowEngine.Infrastructure # Persistence layer (EF Core + SQLite)
├─ WorkflowEngine.Core         # Application/business logic services
├─ WorkflowEngine.Api          # HTTP API + authentication + Swagger
└─ WorkflowEngine.Runtime      # In-memory node registry & execution engine
```

Each layer has a single responsibility and depends only on layers below it (Domain at the bottom, API & Runtime at the top).

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- EF Core CLI (`dotnet tool install --global dotnet-ef --version 8.*`, optional)  
- Git (to clone the repo)

---

## Build & Run

1. **Clone** the repository:
   ```bash
   git clone <repo-url>
   cd <repo-folder>
   ```
2. **Restore & build** all projects:
   ```bash
   dotnet build
   ```
3. **Configure the database** (see Infrastructure README):
   - Ensure `WorkflowEngine.Api/Data` exists
   - Update connection string in `WorkflowEngine.Api/appsettings.json`
4. **Run EF migrations**:
   ```bash
   dotnet ef migrations add InitialCreate \
     --project WorkflowEngine.Infrastructure \
     --startup-project WorkflowEngine.Api
   dotnet ef database update \
     --project WorkflowEngine.Infrastructure \
     --startup-project WorkflowEngine.Api
   ```
5. **Launch the API**:
   ```bash
   dotnet run --project WorkflowEngine.Api
   ```
6. **Browse** Swagger at `http://localhost:5015/swagger` to test endpoints.

---

## Project READMEs

Each project contains its own README with detailed instructions and design notes:

- **Domain**: Business model & contracts  
- **Infrastructure**: EF Core schema, migrations & persistence  
- **Core**: Service interfaces, DTOs, and implementations  
- **API**: REST endpoints, DI, authentication, Swagger  
- **Runtime**: Node registry & execution engine bootstrap

Refer to those for module‑specific guidance.

---

## Next Steps

- **Front‑end UI**: Build a React-based canvas (using React Flow/Vite) to drag‑and‑drop node instances and wire them together.  
- **Graph wiring**: Persist and expose node connections via new endpoints.  
- **Execution engine**: Implement `WorkflowExecutor` to traverse graphs, evaluate conditions, and invoke node operations.  
- **Advanced triggers**: Integrate schedulers, webhooks, and custom event sources.  
- **Deployment**: Swap SQLite for a cloud DB (Azure SQL, PostgreSQL) and enable JWT for public use.

---

*Welcome aboard! Let’s automate the world one node at a time.*