// <<imports>>
import React, { useState } from 'react';
import { nodeRegistry } from '../nodes';
import './ConfigDrawer.css';           // optional styling file

export default function ConfigDrawer({ node, onSave, onClose }) {
  if (!node) return null;              // drawer hidden

  const Def = nodeRegistry[node.type];
  const [config, setConfig] = useState(node.data.configurationJson || '{}');

  return (
    <div className="drawer">
      <header>
        <h3>{Def.name} Configuration</h3>
      </header>

      <div className="drawer-body">
        {/* Node-specific config UI */}
        <Def.ConfigForm
          nodeId={node.id}
          configurationJson={config}
          onChange={setConfig}
        />
      </div>

      <footer className="drawer-actions">
        <button onClick={() => onSave(node.id, config)}>Save</button>
        <button onClick={onClose}>Close</button>
      </footer>
    </div>
  );
}
