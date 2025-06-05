# Overview

Based on the contents of the currently developed ui, all that you need to do is run "npm install", and a new node_modules will be installed.
If you run this app, you need to make sure that the server is running on http://localhost:5015

# Workflow Engine Frontend (UI)

A Vite-powered React app, inspired by Make.com’s scenario editor.  
Drag-and-drop ***nodes*** on a canvas, click to configure them in a pop-over, and persist workflow state to a .NET Core backend.

---

## ✨ Key Features

| Feature | What it does |
|---------|--------------|
| **React-Flow Canvas** | Renders nodes & edges; handles pan/zoom, mini-map, and viewport controls. |
| **Dynamic Nodes** | Node types come from `GET /api/nodes`. The palette (`＋`) lets you spawn new instances. |
| **In-place Config Pop-over** | Click a node to edit its JSON config without leaving the canvas. |
| **Live Persistence** | Drag-stop issues `PUT /api/projects/{pid}/nodeinstances/{id}` to store new coordinates. |
| **Trash-can Delete** | Quickly delete a node; it vanishes from both UI & DB. |
| **Mini-Map & Grid** | Mini-map in the corner for big workflows; background grid aids alignment. |

---

## 🚀 Quick Start

```bash
# 1  Install deps
1. npm install

# 2  Run the dev server (http://localhost:5173)
2. npm run dev

# 3  Build for production
3. npm run build
```

### Prerequisite: the backend API (WorkflowEngine.Api) must be running on
http://localhost:5015 or whatever you set in vite.config.js → proxy.

## 📁 Intended Project Structure (while developing. This may change!)
This structuire is for reference and guidance regarding the intended file and folder structure.

client/                                      # React + Vite front-end root
├── src/                                     # All application source code
│   │
│   ├── assets
│   │   └── react.svg
│   ├── api/                                 # Thin wrappers around REST endpoints
│   │   └── workflowengine.js                # safeFetch(), CRUD helpers for projects / nodes
│   ├── components/                          # Re-usable view components
│   │   ├── ApiStatusBanner.css              # Styles for API-online/offline ribbon
│   │   ├── ApiStatusBanner.jsx              # Shows “API offline” when fetch fails
│   │   ├── Canvas.css                       # Layout tweaks for the React-Flow canvas
│   │   ├── Canvas.jsx                       # Main React-Flow wrapper (nodes / edges / events)
│   │   ├── ConfigPopover.css                # Pop-over styling (shadow, arrows, etc.)
│   │   ├── ConfigPopover.jsx                # JSON editor that hugs the selected node
│   │   ├── NodePalette.jsx                  # Floating dialog listing node types (“＋” button)
│   │   └── NodeRenderer.jsx                 # Custom node box + delete button
│   ├── hooks/
│   │   └── useApiStatus.js                  # Polls backend; drives ApiStatusBanner
│   ├── nodes/                               # One folder per node type + registry
│   │   ├── conditionalBranch/
│   │   │   └── ConditionalBranchNode.jsx   # Renders ⚖️ condition block
│   │   ├── csvReader/                      # (placeholder – coming soon)
│   │   ├── httpRequest/
│   │   │   ├── HttpRequestConfig.jsx        # Config form for HTTP node
│   │   │   ├── HttpRequestNode.css          # Node-specific styles
│   │   │   └── HttpRequestNode.jsx          # GET/POST icon + status badge
│   │   └── index.tsx                        # `nodeRegistry` → maps id ➜ renderer + label
│   ├── pages/
│   │   ├── WorkflowPage.css                 # Page-level layout (toolbar, dark bg)
│   │   └── WorkflowPage.jsx                 # Route wrapper that hosts <Canvas/>
│   ├── types/                               # Tiny TS helper types (optional)
│   │   ├── node.ts                          # Node DTO interface
│   │   └── port.ts                          # Port DTO interface
│   ├── utils/
│   │   └── prefs.js                         # LocalStorage helpers (e.g., last project id)
│   ├── App.css                              # Global CSS reset + font imports
│   ├── App.jsx                              # React entry (layout + router)
│   ├── index.css                            # Tailwind / custom vars (imported in main.jsx)
│   └── main.jsx                             # `createRoot` + <App/>
├── .gitignore                               # Node / log / build artefacts to skip
├── eslint.config.js                         # Lint rules (JSX + hooks)
├── index.html                               # Vite template injected with bundle
├── package.json                             # NPM scripts + dependencies
├── package-lock.json                        # Exact package versions
├── README.md               # 
└── vite.config.js                           # Vite dev-server & proxy to backend


## 🛣️ Data Flow (Drag-Stop Sequence)
User drags node ──┐
                  │ 1  handleDragStop (Canvas.jsx)
                  ▼
update UI state   setNodes(...)
                  ▼
4  PUT /api/projects/{pid}/nodeinstances/{id}
                  │  (body: id, nodeTypeId, configurationJson, positionX/Y)
                  ▼
Backend saves row via EF-Core
                  ▼
204 No Content ◄──┘

## ➕ Extending Node Types
1. Add a new JSON definition to WorkflowEngine.Api/Configuration/NodeDefinitions/.
2. Implement its React renderer in src/nodes/myNode/Renderer.jsx.
3. Register it in src/nodes/index.tsx:

### Prerequisite: the backend API (WorkflowEngine.Api) must be running on
http://localhost:5015 or whatever you set in vite.config.js → proxy

## 🐞 Debugging Tips
Where to peek	What you’ll see
- DevTools ▶ Network	PUT /nodeinstances payload on drag stop or config save.
- Console (browser)	safeFetch logs errors if !res.ok.
- Console (API)	ASP-NET logs model-binding issues (e.g., missing configurationJson).
- Canvas.jsx	Drop console.log in handleDragStop, onNodeClick to track state.
- ConfigPopover.jsx	useEffect runs whenever node changes—good spot to log props.

## 📝 Scripts
**Command          **Description
npm run dev      Vite dev server + HMR
npm run build    Production build in dist/
npm run preview	 Preview the prod build locally

## 📚 References
- **React Flow docs** : https://reactflow.dev/docs/
- **Vite** : https://vitejs.dev/
- **MDN Fetch API** : https://developer.mozilla.org/docs/Web/API/Fetch_API
- **ASP .NET Core Minimal API** : https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis

## © License
MIT — do what you want, but attribution is appreciated.