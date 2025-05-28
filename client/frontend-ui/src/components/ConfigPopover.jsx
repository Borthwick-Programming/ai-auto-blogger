// <<imports>>
import React, { useEffect, useRef } from 'react';
import { useReactFlow } from 'reactflow';
import { nodeRegistry } from '../nodes';
import './ConfigPopover.css';

export default function ConfigPopover({ node, onSave, onClose }) {
  const ref        = useRef(null);
  const { screenToFlowPosition } = useReactFlow();            // gives viewport transform

  // ---------- calc initial pixel position ----------
  useEffect(() => {
    if (!node || !ref.current) return;

    const { x, y } = screenToFlowPosition(node.position);     // flow coords -> DOM px
    const box      = ref.current.getBoundingClientRect();

    ref.current.style.left = `${x - box.width / 2}px`;
    ref.current.style.top  = `${y + 40}px`;      // 40px below the node

  }, [node, screenToFlowPosition]);

  // ---------- follow dragging ----------
  useEffect(() => {
    if (!node) return;

    const update = ({ detail }) => {
      if (detail.id !== node.id) return;
      const { x, y } = screenToFlowPosition(detail.position);
      ref.current.style.left = `${x - ref.current.offsetWidth / 2}px`;
      ref.current.style.top  = `${y + 40}px`;
    };

    window.addEventListener('nodeDrag', update);
    return () => window.removeEventListener('nodeDrag', update);
  }, [node, screenToFlowPosition]);

  if (!node) return null;
  const Def = nodeRegistry[node.type];

  return (
    <div ref={ref} className="cfg-pop">
      <header>{Def.name} Configuration</header>

      <div className="cfg-body">
        <Def.ConfigForm
          nodeId={node.id}
          configurationJson={node.data.configurationJson}
          onChange={json => onSave(node.id, json, /*silent*/ true)}
        />
      </div>

      <footer>
        <button onClick={() => onSave(node.id, null)}>Save</button>
        <button onClick={onClose}>Close</button>
      </footer>
    </div>
  );
}
