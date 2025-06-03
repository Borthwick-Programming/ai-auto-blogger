// <<node-component>>  ────────────────────────────────────────────
import './HttpRequestNode.css';

// A minimal renderer for the HTTP-Request node
import React from 'react';
import { Handle } from 'reactflow';          // ← v10 / v11 import
// - OR -  import { Handle } from 'react-flow-renderer';  // if you’re on v9

export default function HttpRequestNode({ data }) {
  return (
    <div className="wf-node http-node">
      {/* input */}
      <Handle type="target" position="top"    id="in"  />

      {/* label */}
      <span className="http-label">
        {data.label || 'HTTP Request'}
      </span>

      {/* output */}
      <Handle type="source" position="bottom" id="out" />
    </div>
  );
}
