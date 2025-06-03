// <<imports>>
import React, {
  useCallback,
  useEffect,
  useState,
  useRef,
  useMemo,
} from 'react';

import ReactFlow, {
  ReactFlowProvider,
  useNodesState,
  useEdgesState,
  addEdge,
  MiniMap,
  Controls,
  Background,
} from 'reactflow';

import { nodeTypes } from './NodeRenderer';
import ConfigPopover from './ConfigPopover';
import NodePalette    from './NodePalette';       // ← new floating dialog
import * as api       from '../api/workflowengine';

import 'reactflow/dist/style.css';
import './Canvas.css';

/* ----------------------------------------------------------- */
/* helpers                                                     */
/* ----------------------------------------------------------- */
const dtoToRF = dto => ({
  id: dto.id,
  type: dto.nodeTypeCode,  
  nodeTypeId: dto.nodeTypeId,
  position: { x: dto.positionX, y: dto.positionY },
  data: {
    label: dto.label,
    configurationJson: dto.configurationJson,
    nodeTypeId: dto.nodeTypeId,
  },
});

// Simple centre-of-viewport drop location
const projectCenter = () => ({
  x: window.innerWidth  / 2 - 200,         // tweak offsets to taste
  y: window.innerHeight / 2 - 100,
});

/* ----------------------------------------------------------- */
/* component                                                   */
/* ----------------------------------------------------------- */
export default function Canvas({ projectId }) {
  /* state */
  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);
  const [selNode,     setSelNode]     = useState(null);
  const [showPalette, setShowPalette] = useState(false);

  /* load on project change */
  useEffect(() => {
    if (!projectId) return;

    Promise.all([
      api.getInstances(projectId),
      api.getConnections(projectId),
    ]).then(([inst, con]) => {
      setNodes(inst.map(dtoToRF));
      setEdges(
        con.map(c => ({
          id: c.id,
          source: c.sourceNodeInstanceId,
          target: c.targetNodeInstanceId,
          sourceHandle: c.sourcePortId,
          targetHandle: c.targetPortId,
        }))
      );
    });
  }, [projectId, setNodes, setEdges]);

  /* ---- drag helpers ---- */
  const dragStartRef = useRef({ x: 0, y: 0 });

  const handleDragStart = (_, n) => (dragStartRef.current = n.position);

  const handleDragStop = (_, n) => {
    const moved =
      Math.hypot(
        n.position.x - dragStartRef.current.x,
        n.position.y - dragStartRef.current.y,
      ) > 1;

    if (!moved) return;                   // treat as click

    api.saveInstancePos(
      projectId,
      n.id,
      n.data.nodeTypeId,                  // the exact DB value
      n.position,
      n.data.configurationJson ?? '{}',
    );
  };

  const onNodeDrag = useCallback(
    (_, n) => selNode?.id === n.id && setSelNode(n),
    [selNode],
  );

  /* ---- clicks ---- */
  const onNodeClick = useCallback((_, n) => setSelNode(n), []);

  /* ---- config save ---- */
  const handleSaveConfig = (id, json) => {
    setNodes(ns =>
      ns.map(n =>
        n.id === id ? { ...n, data: { ...n.data, configurationJson: json } } : n,
      ),
    );
    api.updateNodeInstance(
      projectId,
      nodes.find(n => n.id === id) ?? dtoToRF({ id, configurationJson: json }),
    );
    setSelNode(null);
  };

  /* ---- create / delete ---- */
  const handleCreate = def => {
    setShowPalette(false);
    const pos = projectCenter();

    api
      .createNode(projectId, {
        nodeTypeId: def.id,
        configurationJson: '{}',
        positionX: pos.x,
        positionY: pos.y,
      })
      .then(dto => setNodes(ns => [...ns, dtoToRF(dto)]));
  };

  const handleDelete = id =>
    api
      .deleteNode(projectId, id)
      .then(() => setNodes(ns => ns.filter(n => n.id !== id)));

  /* ---- memo: edit callbacks for NodeRenderer ---- */
  const nodeDeleteCb = useMemo(
    () => ({ onDelete: handleDelete }),
    [handleDelete],
  );

  /* ----------------------------------------------------------- */
  /* render                                                      */
  /* ----------------------------------------------------------- */
  return (
    <ReactFlowProvider>
      <div className="rf-wrapper">
        <ReactFlow
          nodes={nodes}
          edges={edges}
          nodeTypes={nodeTypes}  
          onNodesDelete={nodeDeleteCb}
          onNodeDragStart={handleDragStart}
          onNodeDragStop={handleDragStop}
          onNodeDrag={onNodeDrag}
          onNodesChange={onNodesChange}
          onEdgesChange={onEdgesChange}
          onNodeClick={onNodeClick}
          onConnect={params => setEdges(eds => addEdge(params, eds))}
          fitView
        >
          <MiniMap
            pannable
            zoomable
            style={{ height: 120 }}
            nodeColor={n =>
              n.type === 'http-request' ? '#3F51B5' : '#FF9800'
            }
          />
          <Controls />
          <Background gap={16} />
        </ReactFlow>

        {showPalette && (
          <NodePalette
            onSelect={handleCreate}
            onClose={() => setShowPalette(false)}
          />
        )}

        <button className="fab" onClick={() => setShowPalette(true)}>
          ＋
        </button>

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
