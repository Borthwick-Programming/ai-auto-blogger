import React, { useCallback, useEffect, useState } from 'react';
import {
  ReactFlowProvider,
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

import type { UpdateNodeInstanceRequest } from '../api/nodeInstancesApi';

import { createNodeInstance, updateNodeInstance } from '../api/nodeInstancesApi';

import type { ReactFlowInstance } from '@xyflow/react';
import { useRef } from 'react';
import { API_BASE } from '../api/httpBase';

/* ---------- Custom node-data payload ---------- */
interface CanvasNodeData extends Record<string, unknown> {
  configuration: Record<string, unknown>;
  nodeTypeId: string;
  nodeId: string;
  projectId: string;
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
  apiBaseUrl = API_BASE,
}) => {
  /* --- React-Flow state hooks ----------------- */
  const [nodes, setNodes, onNodesChange]   = useNodesState<CanvasNode>([]);
  const [edges, setEdges, onEdgesChange]   = useEdgesState<CanvasEdge>([]);
  const rf = useRef<ReactFlowInstance<CanvasNode, CanvasEdge> | null>(null);

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
          projectId: i.projectId
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

  // addDragHandlers                               // <<drag-drop>>
const onDragOver = useCallback(
  (evt: React.DragEvent) => {
    evt.preventDefault();
    evt.dataTransfer.dropEffect = 'move';
  },
  [],
);

const onDrop = useCallback(
  async (evt: React.DragEvent) => {
    evt.preventDefault();
    const data = evt.dataTransfer.getData('application/reactflow');
    if (!data) return;

    const def = JSON.parse(data) as { id: string; name: string };
    const bounds = (evt.target as HTMLDivElement).getBoundingClientRect();
    
    if (!rf.current) return;
    
    const position = rf.current!.screenToFlowPosition({
      x: evt.clientX - bounds.left,
      y: evt.clientY - bounds.top,
    });

    // 1. Persist to API
    const created = await createNodeInstance(projectId, {
      nodeTypeId: def.id,
      configurationJson: '{}',
      positionX: position.x,
      positionY: position.y,
    });

    // 2. Add to local RF state
    setNodes(ns => [
      ...ns,
      {
        id: created.id,
        type: created.nodeTypeId,
        position,
        data: {
          projectId:     projectId,
          configuration: {},
          nodeTypeId: created.nodeTypeId,
          nodeId: created.id,
        },
      },
    ]);
  },
  [projectId, setNodes],
);

/* hook to save position changes ------------- */  // <<drag-drop>>
const onNodeDragStop = async (_: any, node: CanvasNode) => {
  try {
    const { configuration, nodeTypeId } = node.data;
    const body: UpdateNodeInstanceRequest = {
      id: node.id,
      nodeTypeId,
      configurationJson: JSON.stringify(configuration),
      positionX: node.position.x,
      positionY: node.position.y,
    };
    await updateNodeInstance(projectId, node.id, body);
  } catch (err) {
    console.error('Failed to save node-position:', err);
  }
};

  if (isLoading) return <div>Loading workflow…</div>;
  if (error)     return <div>Error: {error}</div>;

  return (
    <div className="canvas">
      <ReactFlowProvider>
      <ReactFlow style={{ width: '100%', height: '100%' }} 
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        onConnect={(params: Connection) => setEdges(eds => addEdge(params, eds))}
        connectionMode={ConnectionMode.Loose}
        fitView
        onInit={(instance) => (rf.current = instance)}
        nodeTypes={NODE_TYPES}
        edgeTypes={EDGE_TYPES}
        
        onDragOver={onDragOver}          // <<drag-drop>>
        onDrop={onDrop}                  // <<drag-drop>>
        onNodeDragStop={onNodeDragStop}  // <<drag-drop>>
      >
        <Controls />
        <Background variant={BackgroundVariant.Dots} gap={12} size={1} />
      </ReactFlow>
      </ReactFlowProvider>
    </div>
  );
};

export default WorkflowCanvas;

/* ---------- API shapes ---------- */
interface NodeInstance {
  id: string;
  projectId: string; 
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
