// src/components/nodes/HttpRequestNode.jsx
import React from 'react';
import { Handle } from 'react-flow-renderer';

export default function HttpRequestNode({ data }) {
  return (
    <div
      style={{
        padding: '8px 12px',
        border: '2px solid #007acc',
        borderRadius: 6,
        background: '#fff',
        minWidth: 120,
        textAlign: 'center',
        fontWeight: 'bold',
        color: '#007acc',
      }}
    >
      {data.label}
      <Handle type="target" position="top" id="in" style={{ background: '#007acc' }} />
      <Handle type="source" position="bottom" id="out" style={{ background: '#007acc' }} />
    </div>
  );
}
