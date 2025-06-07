// src/components/nodes/AffiliateLinkInput/ConfigPopover.tsx
import React, { useState } from 'react';
import { useReactFlow } from '@xyflow/react';
import { updateNodeInstance } from '../../../api/nodeInstancesApi';

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
    // 1. Persist to API
    await updateNodeInstance(
      rf.getNodes()[0].data.projectId as string, nodeId, {
      configurationJson: JSON.stringify(payload),
    });
    // 2. Update local RF state
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
    <div
      style={{
        position: 'absolute',
        top: '-8px',
        left: '160px',
        width: 320,
        background: '#1f1f1f',
        color: '#eee',
        padding: 12,
        borderRadius: 6,
        boxShadow: '0 4px 12px rgba(0,0,0,0.4)',
        zIndex: 20,
      }}
    >
      <h4 style={{ margin: '0 0 8px' }}>Affiliate Link Input</h4>

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
            placeholder="One URL per line, optionally | note"
            style={{ width: '100%', height: 100, marginTop: 8 }}
/* new */
value={links.map((l: LinkEntry) => `${l.url}|${l.note}`).join('\n')}
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
/* old:
            value={links.map(l => `${l.url}|${l.note}`).join('\n')}
            onChange={e =>
              setLinks(
                e.target.value
                  .split('\n')
                  .filter(Boolean)
                  .map(row => {
                    const [url, note = ''] = row.split('|');
                    return { url: url.trim(), note: note.trim() };
                  })
              )
            }
            */

          />
        </>
      ) : (
        <>
          <input
            type="text"
            placeholder="c:\\links\\my_links.csv"
            style={{ width: '100%', marginTop: 8 }}
            value={csvPath}
            onChange={e => setCsvPath(e.target.value)}
          />
          <p style={{ fontSize: 12, color: '#aaa' }}>
            CSV must have two columns: url, note
          </p>
        </>
      )}

      <div style={{ textAlign: 'right', marginTop: 8 }}>
        <button onClick={onClose} style={{ marginRight: 6 }}>
          Cancel
        </button>
        <button onClick={save}>Save</button>
      </div>
    </div>
  );
};

export default ConfigPopover;
