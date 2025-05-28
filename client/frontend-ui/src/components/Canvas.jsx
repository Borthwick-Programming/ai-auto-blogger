// <<imports>>
import React, { useCallback, useEffect, useState } from 'react';
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

  /* ───── drag-stop → persist position ───── */
  const onNodeDragStop = useCallback((e, node) => {
    api.saveInstancePos(projectId, node.id, node.type, node.position);
  }, [projectId]);

  /* ───── clicks ───── */
  const onNodeClick = useCallback((e, node) => {
    setSelNode(node);
  }, []);

  /* ───── save config ───── */
  const handleSaveConfig = (id, json) => {
    setNodes(nds => nds.map(n =>
      n.id === id ? { ...n, data: { ...n.data, configurationJson: json } } : n
    ));
    // TODO: call backend PUT for configurationJson here
    setSelNode(null);
  };

  return (
    <ReactFlowProvider>
      <div className="rf-wrapper">
        <ReactFlow
          nodes={nodes}
          edges={edges}
          nodeTypes={nodeTypes}
          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}
          onNodeDrag={(_, dragNode) => {
            window.dispatchEvent(new CustomEvent('nodeDrag', { detail: dragNode }));
          }}
          onNodeDragStop={onNodeDragStop}
          onNodeClick={onNodeClick}
          onConnect={params => setEdges(eds => addEdge(params, eds))}
          fitView
        />

        <ConfigPopover
          node={selNode}
          onSave={handleSaveConfig}
          onClose={() => setSelNode(null)}
        />
      </div>
    </ReactFlowProvider>
  );
}
