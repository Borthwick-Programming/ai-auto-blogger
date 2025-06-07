import React, { useState } from 'react';
import { Handle, Position } from '@xyflow/react';
import type { NodeProps } from '@xyflow/react'; 

import ConfigPopover from './ConfigPopover';

const box: React.CSSProperties = {
  padding: 8,
  background: '#323232',
  color: '#fff',
  borderRadius: 6,
  border: '1px solid #555',
  minWidth: 140,
  textAlign: 'center',
};

export default function AffiliateLinkInputNode({ selected, id, data }: NodeProps) {
  const [open, setOpen] = useState(false);

  return (
    <>
      <div
        style={{
          ...box,
          border: selected ? '2px solid #7d7dff' : box.border,
        }}
        onDoubleClick={() => setOpen(true)}
      >
        Affiliate Links
      </div>
      <Handle type="source" position={Position.Right} id="out" />
      {open && (
        <ConfigPopover
          nodeId={id}
          initial={data.configuration}
          onClose={() => setOpen(false)}
        />
      )}
    </>
  );
}
