// src/components/Canvas.tsx

// <<canvas-imports>>
import React from 'react';
import ReactFlow, {
  useNodesState,
  useEdgesState,
} from 'reactflow';

// Import all types (including OnConnect) as type-only:
import type {
  Node,
  Edge,
  NodeChange,
  EdgeChange,
  OnNodesChange,
  OnEdgesChange,
  OnConnect,
} from 'reactflow';

import 'reactflow/dist/style.css';

/**
 * A simple Canvas component that renders a React-Flow canvas
 * with one dummy node.
 */
export default function Canvas() {
  // <<initial-nodes>>
  // Define a single dummy node to show on the canvas:
  const initialNodes: Node[] = [
    {
      id: '1',
      type: 'default',               // default node type (a simple box)
      position: { x: 0, y: 0 },       // top-left corner of canvas
      data: { label: 'Dummy Node' },  // the text label inside the node
    },
  ];

  // <<initial-edges>>
  // No edges (connections) yet:
  const initialEdges: Edge[] = [];

  // <<nodes-state>>
  // useNodesState gives us:
  //  • nodes: current array of Node objects
  //  • setNodes: a setter to replace them (not usually needed directly)
  //  • onNodesChange: a handler we pass to <ReactFlow> to auto-update positions, etc.
  const [nodes, , onNodesChange] = useNodesState(initialNodes);

  // <<edges-state>>
  // useEdgesState similarly manages edges for you:
  const [edges, , onEdgesChange] = useEdgesState(initialEdges);

  // <<on-connect-handler>>
  // OnConnect is a type-only export now, so we use it to annotate our handler:
  const onConnect: OnConnect = (connection) => {
    // In the future: add new edge to the list
    // (for now, we’re not drawing edges, so no-op)
  };

  // <<canvas-render>>
  // Return the ReactFlow component inside a full-viewport wrapper:
  return (
    <div style={{ width: '100%', height: '100vh' }}>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange as OnNodesChange}
        onEdgesChange={onEdgesChange as OnEdgesChange}
        onConnect={onConnect}
        fitView={true}           // automatically zoom/pan so all nodes are visible
      />
    </div>
  );
}
