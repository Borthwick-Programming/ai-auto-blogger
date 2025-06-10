// src/components/nodes/AffiliateLinkInput/ConfigPopover.tsx
import {
  listPrePrompts,
  createPrePrompt,
  updatePrePrompt,
  deletePrePrompt,
} from '../../../api/prePromptsApi';
import React, { useState, useRef, useEffect } from 'react';
import { useReactFlow } from '@xyflow/react';
import { updateNodeInstance } from '../../../api/nodeInstancesApi';
import type { UpdateNodeInstanceRequest } from '../../../api/nodeInstancesApi';
import type { PrePrompt } from '../../../api/prePromptsApi';

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

  /* pre prompts */
  const [prePrompts, setPrePrompts]       = useState<PrePrompt[]>([]);
  const [showPromptModal, setShowPromptModal] = useState(false);
  const [editingPrompt, setEditingPrompt] = useState<PrePrompt | null>(null);
  const [newPrompt, setNewPrompt]         = useState({ name: '', promptText: '' });

  useEffect(() => {
    listPrePrompts().then(setPrePrompts);
  }, []);

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

<button type="button" className="manage-prompts" onClick={() => setShowPromptModal(true)} >
Manage Pre-Prompts
</button>

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

{showPromptModal && (
  <div className="preprompt-modal">
    <div className="modal-content">
      <h5>Pre-Prompts</h5>
      <ul>
        {prePrompts.map(pp => (
          <li key={pp.id} className="pp-row">
            <input
              value={pp.name}
              onChange={e => {
                const updated = { ...pp, name: e.target.value };
                setPrePrompts(pl =>
                  pl.map(x => x.id === pp.id ? updated : x)
                );
                setEditingPrompt(updated);
              }}
            />
            <textarea
              value={pp.promptText}
              onChange={e => {
                const updated = { ...pp, promptText: e.target.value };
                setPrePrompts(pl =>
                  pl.map(x => x.id === pp.id ? updated : x)
                );
                setEditingPrompt(updated);
              }}
            />
            <button
              onClick={async () => {
                if (editingPrompt?.id === pp.id) await updatePrePrompt(editingPrompt);
              }}
            >
              Save
            </button>
            <button
              onClick={async () => {
                await deletePrePrompt(pp.id);
                setPrePrompts(pl => pl.filter(x => x.id !== pp.id));
              }}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>

      <h5>Add New</h5>
      <input
        placeholder="Name"
        value={newPrompt.name}
        onChange={e => setNewPrompt(np => ({ ...np, name: e.target.value }))}
      />
      <textarea
        placeholder="Prompt text"
        value={newPrompt.promptText}
        onChange={e => setNewPrompt(np => ({ ...np, promptText: e.target.value }))}
      />
      <button
        onClick={async () => {
          const created = await createPrePrompt(newPrompt);
          setPrePrompts(pl => [...pl, created]);
          setNewPrompt({ name: '', promptText: '' });
        }}
      >
        Add
      </button>

      <button onClick={() => setShowPromptModal(false)}>Close</button>
    </div>
  </div>
)}


        </div>
  );
};

export default ConfigPopover;
