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
} from 'reactflow';
import type {
  Node,
  Edge,
  Connection,
  NodeTypes,
} from 'reactflow';
import 'reactflow/dist/style.css';

// Custom Node Components
import HttpRequestNode from './nodes/HttpRequestNode';
import ConditionalBranchNode from './nodes/ConditionalBranchNode';

// Types based on your API
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

interface WorkflowCanvasProps {
  projectId: string;
  apiBaseUrl?: string;
}

const nodeTypes: NodeTypes = {
  'http-request': HttpRequestNode,
  'conditional-branch': ConditionalBranchNode,
};

const WorkflowCanvas: React.FC<WorkflowCanvasProps> = ({ 
  projectId, 
  apiBaseUrl = 'http://localhost:5000' // Adjust to your API URL
}) => {
  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Load workflow data from your API
  const loadWorkflow = useCallback(async () => {
    try {
      setIsLoading(true);
      setError(null);

      // Fetch node instances
      const nodesResponse = await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeinstances`);
      if (!nodesResponse.ok) throw new Error('Failed to load nodes');
      const nodeInstances: NodeInstance[] = await nodesResponse.json();

      // Fetch node connections
      const connectionsResponse = await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeconnections`);
      if (!connectionsResponse.ok) throw new Error('Failed to load connections');
      const nodeConnections: NodeConnection[] = await connectionsResponse.json();

      // Convert API data to ReactFlow format
      const flowNodes: Node[] = nodeInstances.map(instance => ({
        id: instance.id,
        type: instance.nodeTypeId,
        position: { x: instance.positionX, y: instance.positionY },
        data: {
          configuration: instance.configurationJson ? JSON.parse(instance.configurationJson) : {},
          nodeTypeId: instance.nodeTypeId,
          onConfigChange: (config: any) => updateNodeConfiguration(instance.id, config),
        },
      }));

      const flowEdges: Edge[] = nodeConnections.map(connection => ({
        id: connection.id,
        source: connection.fromNodeInstanceId,
        target: connection.toNodeInstanceId,
        sourceHandle: connection.fromPortName,
        targetHandle: connection.toPortName,
      }));

      setNodes(flowNodes);
      setEdges(flowEdges);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load workflow');
    } finally {
      setIsLoading(false);
    }
  }, [projectId, apiBaseUrl]);

  // Save node position when dragged
  const handleNodeDragStop = useCallback(async (event: any, node: Node) => {
    try {
      const updateRequest = {
        id: node.id,
        nodeTypeId: node.data.nodeTypeId,
        configurationJson: JSON.stringify(node.data.configuration),
        positionX: node.position.x,
        positionY: node.position.y,
      };

      await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeinstances/${node.id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updateRequest),
      });
    } catch (err) {
      console.error('Failed to save node position:', err);
    }
  }, [projectId, apiBaseUrl]);

  // Update node configuration
  const updateNodeConfiguration = useCallback(async (nodeId: string, configuration: any) => {
    try {
      const node = nodes.find(n => n.id === nodeId);
      if (!node) return;

      const updateRequest = {
        id: nodeId,
        nodeTypeId: node.data.nodeTypeId,
        configurationJson: JSON.stringify(configuration),
        positionX: node.position.x,
        positionY: node.position.y,
      };

      await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeinstances/${nodeId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(updateRequest),
      });

      // Update local state
      setNodes(prevNodes =>
        prevNodes.map(n =>
          n.id === nodeId
            ? { ...n, data: { ...n.data, configuration } }
            : n
        )
      );
    } catch (err) {
      console.error('Failed to update node configuration:', err);
    }
  }, [nodes, projectId, apiBaseUrl]);

  // Handle new connections
  const onConnect = useCallback(async (connection: Connection) => {
    if (!connection.source || !connection.target) return;

    try {
      const createRequest = {
        fromNodeInstanceId: connection.source,
        fromPortName: connection.sourceHandle || '',
        toNodeInstanceId: connection.target,
        toPortName: connection.targetHandle || '',
      };

      const response = await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeconnections`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(createRequest),
      });

      if (!response.ok) throw new Error('Failed to create connection');
      
      const newConnection = await response.json();
      
      const newEdge: Edge = {
        id: newConnection.id || `${connection.source}-${connection.target}`,
        source: connection.source,
        target: connection.target,
        sourceHandle: connection.sourceHandle,
        targetHandle: connection.targetHandle,
      };

      setEdges(eds => addEdge(newEdge, eds));
    } catch (err) {
      console.error('Failed to create connection:', err);
    }
  }, [projectId, apiBaseUrl]);

  // Delete connection
  const onEdgesDelete = useCallback(async (edgesToDelete: Edge[]) => {
    for (const edge of edgesToDelete) {
      try {
        await fetch(`${apiBaseUrl}/api/projects/${projectId}/nodeconnections/${edge.id}`, {
          method: 'DELETE',
        });
      } catch (err) {
        console.error('Failed to delete connection:', err);
      }
    }
  }, [projectId, apiBaseUrl]);

  // Load workflow on mount
  useEffect(() => {
    loadWorkflow();
  }, [loadWorkflow]);

  if (isLoading) {
    return (
      <div className="workflow-canvas-loading">
        <div>Loading workflow...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="workflow-canvas-error">
        <div>Error: {error}</div>
        <button onClick={loadWorkflow}>Retry</button>
      </div>
    );
  }

  return (
    <div style={{ width: '100%', height: '100vh' }}>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        onConnect={onConnect}
        onNodeDragStop={handleNodeDragStop}
        onEdgesDelete={onEdgesDelete}
        nodeTypes={nodeTypes}
        connectionMode={ConnectionMode.Loose}
        fitView
      >
        <Controls />
        <Background variant={BackgroundVariant.Dots} gap={12} size={1} />
      </ReactFlow>
    </div>
  );
};

export default WorkflowCanvas;