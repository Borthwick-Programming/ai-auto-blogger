// src/components/NodePalette.tsx   // <<NodePalette>>
import React, { useEffect, useState } from 'react';
import { getAllNodeTypes } from '../api/nodeTypesApi';

interface NodeDef {
  id: string;
  name: string;
  icon?: string;
  reactComponent: string;
}

const NodePalette: React.FC = () => {
  const [nodes, setNodes] = useState<NodeDef[]>([]);

  useEffect(() => { getAllNodeTypes().then(setNodes); }, []);

  /* ---------- drag helpers ---------- */
  const onDragStart = (evt: React.DragEvent, node: NodeDef) => {
    evt.dataTransfer.setData('application/reactflow', JSON.stringify(node));
    evt.dataTransfer.effectAllowed = 'move';
  };
  /* ---------------------------------- */

  return (
    <aside className="palette">
      <h3>Nodes</h3>
      {nodes.map(n => (
        <div key={n.id}
             className="palette-item"
             draggable
             onDragStart={evt => onDragStart(evt, n)}>
          <span className={`icon-${n.icon ?? 'circle'}`} />
          {n.name}
        </div>
      ))}
    </aside>
  );
};

export default NodePalette;
