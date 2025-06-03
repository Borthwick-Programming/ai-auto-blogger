import React from 'react';
import { Handle } from 'reactflow';
export default function ConditionalBranchNode({ data }) {
  return (
    <div className="wf-node cond-node">
      <Handle type="target" position="top" id="in" />
      <span className="cond-label">
        {data.label || 'Conditional'}
      </span>
      <Handle type="source" position="bottom" id="true"  style={{ left: '30%' }} />
      <Handle type="source" position="bottom" id="false" style={{ left: '70%' }} />
    </div>
  );
}