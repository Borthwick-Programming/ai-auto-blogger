import React, { useCallback, useEffect, useState } from 'react';
import {
  ReactFlow,
  addEdge,
  useNodesState,
  useEdgesState,
  Controls,
  Background,
  BackgroundVariant,
  ConnectionMode,
  type Node,
  type Edge,
  type Connection,
} from '@xyflow/react';

import '@xyflow/react/dist/style.css';

import { NODE_TYPES, EDGE_TYPES } from './nodeRegistry';

/* ---------- Custom node-data payload ---------- */
interface CanvasNodeData extends Record<string, unknown> {
  configuration: Record<string, unknown>;
  nodeTypeId: string;
  nodeId: string;
}

type CanvasNode = Node<CanvasNodeData>;
type CanvasEdge = Edge;
/* --------------------------------------------- */

/* ---------- Props coming from parent ---------- */
interface WorkflowCanvasProps {
  projectId: string;
  apiBaseUrl?: string;
}
/* --------------------------------------------- */

const WorkflowCanvas: React.FC<WorkflowCanvasProps> = ({
  projectId,
  apiBaseUrl = 'http://localhost:5173',
}) => {
  /* --- React-Flow state hooks ----------------- */
  const [nodes, setNodes, onNodesChange]   = useNodesState<CanvasNode>([]);
  const [edges, setEdges, onEdgesChange]   = useEdgesState<CanvasEdge>([]);
  /* ------------------------------------------- */

  const [isLoading, setIsLoading] = useState(true);
  const [error,      setError]    = useState<string | null>(null);

  /* ---------- Load workflow from API ---------- */
  const loadWorkflow = useCallback(async () => {
    try {
      setIsLoading(true);
      setError(null);

      /* 1. Node instances */
      const nRes   = await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeinstances`);
      if (!nRes.ok) throw new Error('Failed to load nodes');
      const nodeInstances = await nRes.json() as NodeInstance[];

      /* 2. Connections */
      const cRes   = await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeconnections`);
      if (!cRes.ok) throw new Error('Failed to load connections');
      const nodeConnections = await cRes.json() as NodeConnection[];

      /* 3. Transform to React-Flow types */
      const flowNodes: CanvasNode[] = nodeInstances.map(i => ({
        id: i.id,
        type: i.nodeTypeId,
        position: { x: i.positionX, y: i.positionY },
        data: {
          configuration: i.configurationJson ? JSON.parse(i.configurationJson) : {},
          nodeTypeId: i.nodeTypeId,
          nodeId: i.id,
        },
      }));

      const flowEdges: CanvasEdge[] = nodeConnections.map(c => ({
        id: c.id,
        source: c.fromNodeInstanceId,
        target: c.toNodeInstanceId,
        sourceHandle: c.fromPortName,
        targetHandle: c.toPortName,
      }));

      setNodes(flowNodes);
      setEdges(flowEdges);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load workflow');
    } finally {
      setIsLoading(false);
    }
  }, [apiBaseUrl, projectId, setNodes, setEdges]);
  /* ------------------------------------------- */

  useEffect(() => { loadWorkflow(); }, [loadWorkflow]);

  if (isLoading) return <div>Loading workflow…</div>;
  if (error)     return <div>Error: {error}</div>;

  return (
    <div style={{ width: '100%', height: '100vh' }}>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        onConnect={(params: Connection) => setEdges(eds => addEdge(params, eds))}
        connectionMode={ConnectionMode.Loose}
        fitView
        nodeTypes={NODE_TYPES}
        edgeTypes={EDGE_TYPES}
      >
        <Controls />
        <Background variant={BackgroundVariant.Dots} gap={12} size={1} />
      </ReactFlow>
    </div>
  );
};

export default WorkflowCanvas;

/* ---------- API shapes ---------- */
interface NodeInstance {
  id: string;
  nodeTypeId: string;
  configurationJson: string;
  positionX: number;
  positionY: number;
}

interface NodeConnection {
  id: string;
  fromNodeInstanceId: string;
  fromPortName: string;
  toNodeInstanceId: string;
  toPortName: string;
}
/* -------------------------------- */
