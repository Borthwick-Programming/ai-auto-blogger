import React, { useState, useEffect, useCallback } from 'react';
import ReactFlow, {
  addEdge,
  MiniMap,
  Controls,
  Background,
} from 'react-flow-renderer';

export function WorkflowCanvas({ projectId }) {
  const [nodes, setNodes] = useState([]);
  const [edges, setEdges] = useState([]);

  // Load node instances
  useEffect(() => {
    Promise.all([
      fetch(`/api/projects/${projectId}/nodeinstances`).then(r => r.json()),
      fetch(`/api/projects/${projectId}/nodeconnections`).then(r => r.json()),
    ]).then(([instances, connections]) => {
      setNodes(
        instances.map(n => ({
          id: n.id,
          type: 'default',
          position: { x: n.positionX, y: n.positionY },
          data: { label: n.nodeTypeId },
        }))
      );
      setEdges(
        connections.map(c => ({
          id: c.id,
          source: c.fromNodeInstanceId,
          sourceHandle: c.fromPortName,
          target: c.toNodeInstanceId,
          targetHandle: c.toPortName,
        }))
      );
    });
  }, [projectId]);

  // When you drag a node, persist its new position
  const onNodeDragStop = useCallback((_, node) => {
    setNodes(nds =>
      nds.map(n => (n.id === node.id ? { ...n, position: node.position } : n))
    );
    fetch(`/api/projects/${projectId}/nodeinstances/${node.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        id: node.id,
        nodeTypeId: node.data.label,
        configurationJson: '{}', // preserve existing config
        positionX: node.position.x,
        positionY: node.position.y,
      }),
    });
  }, [projectId]);

  // When you draw a connection, post it to the API
  const onConnect = useCallback(connection => {
    setEdges(es => addEdge(connection, es));
    fetch(`/api/projects/${projectId}/nodeconnections`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        fromNodeInstanceId: connection.source,
        fromPortName: connection.sourceHandle,
        toNodeInstanceId: connection.target,
        toPortName: connection.targetHandle,
      }),
    })
      .then(r => r.json())
      .then(saved => {
        // swap the temporary edge id for the real one
        setEdges(es =>
          es.map(e =>
            e.id === connection.id ? { ...e, id: saved.id } : e
          )
        );
      });
  }, [projectId]);

  return (
    <div style={{ width: '100%', height: '80vh' }}>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodeDragStop={onNodeDragStop}
        onConnect={onConnect}
        fitView
      >
        <MiniMap />
        <Controls />
        <Background />
      </ReactFlow>
    </div>
  );
}
