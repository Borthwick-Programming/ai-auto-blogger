// src/components/nodes/ConditionalBranchNode.jsx
import React from 'react';
import { Handle } from 'react-flow-renderer';

export default function ConditionalBranchNode({ data }) {
  return (
    <div
      style={{
        padding: '8px 12px',
        border: '2px solid #f08a24',
        borderRadius: 6,
        background: '#fff',
        minWidth: 120,
        textAlign: 'center',
        fontWeight: 'bold',
        color: '#f08a24',
      }}
    >
      {data.label}
      <Handle type="target" position="top" id="in" style={{ background: '#f08a24' }} />
      {/* two output ports, e.g. yes/no */}
      <Handle type="source" position="bottom" id="yes" style={{ background: '#f08a24', left: 30 }} />
      <Handle type="source" position="bottom" id="no"  style={{ background: '#f08a24', right: 30 }} />
    </div>
  );
}
