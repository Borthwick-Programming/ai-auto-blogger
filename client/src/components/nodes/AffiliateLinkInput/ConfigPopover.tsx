// src/components/nodes/AffiliateLinkInput/ConfigPopover.tsx
import React, { useState, useRef } from 'react';
import { useReactFlow } from '@xyflow/react';
import { updateNodeInstance } from '../../../api/nodeInstancesApi';
import type { UpdateNodeInstanceRequest } from '../../../api/nodeInstancesApi';
import './ConfigPopover.css';

interface Props {
  nodeId: string;
  initial: any;
  onClose: () => void;
}

interface LinkEntry {
  url: string;
  note: string;
}

const ConfigPopover: React.FC<Props> = ({ nodeId, initial, onClose }) => {
  const rf = useReactFlow();
  const [mode, setMode] = useState<'inline' | 'csv'>(initial.source);
  const [links, setLinks] = useState(initial.links ?? []);
  const [csvPath, setCsvPath] = useState(initial.csvPath ?? '');

  /* ----- helpers ----- */
  const save = async () => {
    const payload = {
      source: mode,
      links,
      csvPath,
    };
    const node = rf.getNode(nodeId)!;

    const dto: UpdateNodeInstanceRequest = {
    id:                 nodeId,
    nodeTypeId:         node.data.nodeTypeId as string,
    configurationJson:  JSON.stringify(payload),
    positionX:          node.position.x,
    positionY:          node.position.y,
  };

  await updateNodeInstance(
    node.data.projectId as string,
    nodeId,
    dto,
  );

  rf.setNodes(ns =>
  ns.map(n =>
    n.id === nodeId
      ? { ...n, data: { ...n.data, configuration: payload } }
      : n
  )
);

  onClose();
  
};

  /* ----- UI ----- */
  return (
    <div className="config-popover">
      <h4>Affiliate Link Input</h4>

      <label>
        <input
          type="radio"
          checked={mode === 'inline'}
          onChange={() => setMode('inline')}
        />{' '}
        Inline list
      </label>
      <label style={{ marginLeft: 12 }}>
        <input
          type="radio"
          checked={mode === 'csv'}
          onChange={() => setMode('csv')}
        />{' '}
        CSV file
      </label>

      {mode === 'inline' ? (
        <>
          <textarea
            placeholder="One URL per line, optionally | note" className="url-list" value={links.map((l: LinkEntry) => `${l.url}|${l.note}`).join('\n')}
            onChange={(e) => {
              const rawLines = e.target.value
              .split('\n')
              .filter(Boolean);
              const parsed: LinkEntry[] = rawLines.map((row) => {
                  const [url, note = ''] = row.split('|');
                    return { url: url.trim(), note: note.trim() };
                    });
                      setLinks(parsed);
                      }}
          />
        </>
      ) : (
        <>
          {/* <<CSV file input>> */}
          <div className="file-picker">
            <input
            type="file"
            accept=".csv"
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
              const file = e.target.files?.[0];
              if (file) setCsvPath(file.name);
            }}
            />
            <div className="fake-input">
              {csvPath || 'Select CSV file…'}
              </div>
              </div>
              <p>
                CSV must have two columns: url, note (for the AI to read about what to do)
                </p>
        </>
      )}

      <div className="actions">
        <button onClick={onClose}>Cancel</button>
        <button className="save" onClick={save}>Save</button>
        </div>
        </div>
  );
};

export default ConfigPopover;
