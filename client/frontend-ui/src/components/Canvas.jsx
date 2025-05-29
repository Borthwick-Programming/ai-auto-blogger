// <<imports>>
import React, { useCallback, useEffect, useState, useRef } from 'react';
import { nodeTypes } from './NodeRenderer';
import ReactFlow, {
  ReactFlowProvider,
  useNodesState,
  useEdgesState,
  addEdge,
} from 'reactflow';


//import ConfigDrawer from './ConfigDrawer';
import ConfigPopover from './ConfigPopover';
import * as api from '../api/workflowengine';

import 'reactflow/dist/style.css';   // base RF styles
import './Canvas.css';               // your custom tweaks

export default function Canvas({ projectId }) {

const dragPosRef = useRef({ x: 0, y: 0 });

const handleDragStart = (_, node) => {
  dragPosRef.current = node.position;       // remember where the drag began
};

const handleDragStop = (_, node) => {
  const start = dragPosRef.current;
  const end   = node.position;
  const moved = Math.hypot(end.x - start.x, end.y - start.y);

  if (moved < 1) return;                    // ← treat as click, skip PUT

  api.saveInstancePos
    (
      projectId,
      node.id, 
      node.type,
      end,
      node.data?.configurationJson ?? {},
    );
};

  /* ───── state ───── */
  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);

  const [selNode, setSelNode] = useState(null);

  /* ───── load nodes/edges on project change ───── */
  useEffect(() => {
    if (!projectId) return;

    Promise.all([
      api.getInstances(projectId),
      api.getConnections(projectId),
    ]).then(([inst, con]) => {
      setNodes(inst.map(dto => ({
        id: dto.id,
        type: dto.nodeTypeId,
        position: { x: dto.positionX, y: dto.positionY },
        data: {
          label: dto.label,
          configurationJson: dto.configurationJson,
          nodeType: dto.nodeTypeId,
        },
      })));

      setEdges(con.map(c => ({
        id: c.id,
        source: c.sourceNodeInstanceId,
        target: c.targetNodeInstanceId,
        sourceHandle: c.sourcePortId,
        targetHandle: c.targetPortId,
      })));
    });
  }, [projectId, setNodes, setEdges]);


  /* ───── clicks ───── */
  const onNodeClick = useCallback((e, node) => {
    setSelNode(node);
  }, []);

  /* ───── save config ───── */
  // <<persist-configuration>>
  const handleSaveConfig = (id, json) => {
    setNodes(nds => 
      nds.map(n => n.id === id ? { ...n, data: { ...n.data, configurationJson: json } } : n )
    );
    const n = nodes.find(n => n.id === id);  // node after state update
    if (n) api.updateNodeInstance(projectId, n); //sends nodeTypeId
    setSelNode(null);
  };

  const onNodeDrag = useCallback((_, node) => {
    if (selNode?.id === node.id) setSelNode(node);
    }, [selNode]);

  return (
    <ReactFlowProvider>
      <div className="rf-wrapper">
        <ReactFlow
          nodes={nodes}
          edges={edges}
          nodeTypes={nodeTypes}
          onNodeDragStart={handleDragStart}
          onNodeDragStop={handleDragStop}
          onNodeDrag={onNodeDrag}

          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}

          onNodeClick={onNodeClick}
          onConnect={params => setEdges(eds => addEdge(params, eds))}
          fitView
        />

        <ConfigPopover
          node={selNode}
          projectId={projectId}
          onSave={handleSaveConfig}
          onClose={() => setSelNode(null)}
        />
      </div>
    </ReactFlowProvider>
  );
}
