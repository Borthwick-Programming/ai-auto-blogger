// src/components/WorkflowCanvas.jsx
import React, { useState, useEffect, useCallback} from 'react';
import ReactFlow, { MiniMap, Controls, Background, useNodesState, useEdgesState } from 'react-flow-renderer';
import HttpRequestNode from './nodes/HttpRequestNode';
import ConditionalBranchNode from './nodes/ConditionalBranchNode';
import { Listbox } from '@headlessui/react'
import './WorkflowCanvas.css';

const nodeTypes = {
  'http-request': HttpRequestNode,
  'conditional-branch': ConditionalBranchNode,
};

export default function WorkflowCanvas({ projectId }) {
  const [projects, setProjects] = useState([]);
  const [selectedProject, setSelectedProject] = useState(null);
  const [nodeDefs, setNodeDefs] = useState([]);
  // use controlled state hooks for React Flow

  const [nodes, setNodes, onNodesChange] = useNodesState([]);
   const [edges, setEdges, onEdgesChange] = useEdgesState([]);

   // -- handlers ---------------------------------------------------------
   // drag-stop - PUT new position
const onNodeDragStop = useCallback((_, n) => {
  setNodes(nds =>
    nds.map(x => (x.id === n.id ? { ...x, position: n.position } : x))
    );
    fetch(`/api/projects/${selectedProject}/nodeinstances/${n.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        id: n.id,
        nodeTypeId: n.data.nodeType,
        configurationJson: n.data.configurationJson || '{}',
        positionX: n.position.x,
        positionY: n.position.y,
        }),
        }).catch(console.error);
        }, [selectedProject, setNodes]);

        // delete nodes → DELETE endpoint
const onNodesDelete = useCallback(del => {
  del.forEach(n =>
    fetch(`/api/projects/${selectedProject}/nodeinstances/${n.id}`, {
       method: 'DELETE',
       }).catch(console.error)
       );
       }, [selectedProject]);

       // delete edges → DELETE endpoint
       const onEdgesDelete = useCallback(del => {
        del.forEach(e =>
          fetch(`/api/projects/${selectedProject}/nodeconnections/${e.id}`, {
             method: 'DELETE',
             }).catch(console.error)
             );
             }, [selectedProject]);


   // track clicked node for configuration
    const [configNode, setConfigNode] = useState(null);
    const [configPos, setConfigPos]   = useState({ top: 0, left: 0 });
    //const onNodeClick = useCallback((_, n) => setConfigNode(n), []);
     // capture the node and its screen position relative to  canvas-wrapper
     const onNodeClick = useCallback((event, node) => {
      setConfigNode(node);
      // find the node's DOM element
      const nodeEl = document.querySelector(
         `.react-flow__node[data-id="${node.id}"]`
         );
         const wrapper = document.querySelector('.canvas-wrapper');
         if (nodeEl && wrapper) {
          const nodeRect    = nodeEl.getBoundingClientRect();
          const wrapperRect = wrapper.getBoundingClientRect();
          // position panel just to the right of the node, aligned to its top
          setConfigPos({
            top:  nodeRect.top   - wrapperRect.top,
            left: nodeRect.left  - wrapperRect.left + nodeRect.width + 8,
            });
            }
            }, []);

  // load projects and definitions once
useEffect(() => {
  async function loadLookups() {
    try {
      const projRes = await fetch('/api/projects');
      if (!projRes.ok) throw new Error(`Projects ${projRes.status}`);
      const nodeRes = await fetch('/api/nodes');
      if (!nodeRes.ok) throw new Error(`Nodes ${nodeRes.status}`);

      setProjects(await projRes.json());
      setNodeDefs(await nodeRes.json());
    } catch (err) {
      console.error('Lookup load failed:', err);
      // optional: set an error state and render a message
    }
  }
  loadLookups();
}, []);

  // when project or defs change, load instances + connections
  useEffect(() => {
    if (!selectedProject) return;
    // nodes
    fetch(`/api/projects/${selectedProject}/nodeinstances`)
      .then(r => r.json())
      .then(list =>
        setNodes(
          list.map(n => ({
            id: n.id,
            type: n.nodeTypeId,         // matches our nodeTypes keys
            position: { x: n.positionX, y: n.positionY },
            data: {
              label:
                nodeDefs.find(d => d.nodeType === n.nodeTypeId)?.name ||
                n.nodeTypeId,
               nodeType: n.nodeTypeId,             // <- needed by drag-stop PUT
               configurationJson: n.configurationJson
            },
          }))
        )
      );
    // edges
    fetch(`/api/projects/${selectedProject}/nodeconnections`)
      .then(r => r.json())
      .then(list =>
        setEdges(
          list.map(c => ({
            id: c.id,
            source: c.fromNodeInstanceId,
            sourceHandle: c.fromPortName,
            target: c.toNodeInstanceId,
            targetHandle: c.toPortName,
          }))
        )
      );
  }, [selectedProject, nodeDefs]);

  return (
    <div className="workflow-container">
      <header className="workflow-header">
  <h1 style={{ marginRight: '1rem' }}>Your Workflow Projects</h1>

  {/* Project picker */}
  <Listbox value={selectedProject} onChange={setSelectedProject}>
    <div style={{ position: 'relative' }}>
      {/* The button that shows the current selection */}
      <Listbox.Button className="dropdown-button">
        {selectedProject
          ? projects.find(p => p.id === selectedProject)?.name
          : '— pick a project —'}
      </Listbox.Button>

      {/* The actual dropdown menu */}
      <Listbox.Options className="dropdown-options">
        {projects.map(p => (
          <Listbox.Option
            key={p.id}
            value={p.id}
            className={({ active }) =>
              `dropdown-option ${active ? 'active' : ''}`
            }
          >
            {p.name}
          </Listbox.Option>
        ))}
      </Listbox.Options>
    </div>
  </Listbox>
</header>

      <div className="canvas-wrapper">
        {selectedProject && (
          <ReactFlow
            nodes={nodes}
            edges={edges}
            nodeTypes={nodeTypes}
            onNodesChange={onNodesChange}
            onEdgesChange={onEdgesChange}
           onNodeDragStop={onNodeDragStop}
           onNodesDelete={onNodesDelete}
           onEdgesDelete={onEdgesDelete}
           onNodeClick={onNodeClick}
            fitView
          >
            <MiniMap />
            <Controls />
            <Background />
          </ReactFlow>
          
        )}

        {configNode && (
          <div className="config-panel" style={{ top: configPos.top, left: configPos.left }}>

            <h3>{configNode.data.label} Configuration</h3>
            <p><b>ID:</b> {configNode.id}</p>
            <p><b>Type:</b> {configNode.data.nodeType}</p>

      {/* TODO: replace with a real Form once schemas are ready */}
      {configNode.data.configurationJson && configNode.data.configurationJson !== '{}'? 
      (
        <textarea style={{ width: '100%', height: 80 }} defaultValue={configNode.data.configurationJson}/>
      ) : 
      (
        <p className="no-config">No configuration available.</p>
      )
      }

        <button style={{ marginTop: 8 }}>Save</button>
        <button style={{ marginLeft: 8 }} onClick={() => setConfigNode(null)}>
        Close
        </button>
        </div>
      )}
      </div>
    </div>
  );
}
