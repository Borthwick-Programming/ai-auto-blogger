import React, { useState } from 'react';
import {
  Handle,
  Position,
  useReactFlow,
  type Node,
  type NodeProps,
} from '@xyflow/react';

/* ---------- payload carried in node.data ---------- */
interface HttpRequestNodeData extends Record<string, unknown>{
  configuration: {
    url?: string;
    method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  };
  nodeTypeId: string;
  nodeId: string;
}
/* full React-Flow node type */
type HttpRequestRFNode = Node<HttpRequestNodeData>;
/* -------------------------------------------------- */

const HttpRequestNode: React.FC<NodeProps<HttpRequestRFNode>> = ({
  data,
  selected,
  id,
}) => {
  const [isEditing, setIsEditing] = useState(false);
  const [tempConfig, setTempConfig] = useState(data.configuration);
  const { setNodes } = useReactFlow();

  const handleSave = () => {
    setNodes(nodes =>
      nodes.map(node =>
        node.id === id
          ? { ...node, data: { ...node.data, configuration: tempConfig } }
          : node,
      ),
    );
    setIsEditing(false);
  };

  const handleCancel = () => {
    setTempConfig(data.configuration);
    setIsEditing(false);
  };

  return (
    <div
      className={`http-request-node ${selected ? 'selected' : ''}`}
      style={{
        background: '#3F51B5',
        color: 'white',
        border: selected ? '2px solid #1976D2' : '1px solid #3F51B5',
        borderRadius: '8px',
        padding: '12px',
        minWidth: '200px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
      }}
    >
      {/* Header */}
      <div style={{ display: 'flex', alignItems: 'center', marginBottom: 8 }}>
        <div style={{ marginRight: 8 }}>🌐</div>
        <strong>HTTP Request</strong>
      </div>

      {/* Body */}
      {isEditing ? (
        /* ----- edit mode ----- */
        <div
          style={{
            background: 'rgba(255,255,255,0.1)',
            padding: 8,
            borderRadius: 4,
          }}
        >
          {/* method */}
          <label style={{ fontSize: 12 }}>Method:</label>
          <select
            value={tempConfig.method || 'GET'}
            onChange={e =>
              setTempConfig({
                ...tempConfig,
                method: e.target.value as HttpRequestNodeData['configuration']['method'],
              })
            }
            style={{ width: '100%', marginBottom: 8 }}
          >
            {['GET', 'POST', 'PUT', 'DELETE'].map(m => (
              <option key={m}>{m}</option>
            ))}
          </select>

          {/* url */}
          <label style={{ fontSize: 12 }}>URL:</label>
          <input
            type="text"
            value={tempConfig.url || ''}
            onChange={e =>
              setTempConfig({ ...tempConfig, url: e.target.value })
            }
            placeholder="https://api.example.com/endpoint"
            style={{ width: '100%', marginBottom: 8 }}
          />

          <div style={{ display: 'flex', gap: 4 }}>
            <button style={{ flex: 1 }} onClick={handleSave}>
              Save
            </button>
            <button style={{ flex: 1 }} onClick={handleCancel}>
              Cancel
            </button>
          </div>
        </div>
      ) : (
        /* ----- read-only mode ----- */
        <div
          onClick={() => setIsEditing(true)}
          style={{
            cursor: 'pointer',
            padding: 8,
            background: 'rgba(255,255,255,0.1)',
            borderRadius: 4,
          }}
        >
          <div style={{ fontSize: 12, opacity: 0.8 }}>
            {data.configuration.method || 'GET'}
          </div>
          <div style={{ wordBreak: 'break-all' }}>
            {data.configuration.url || 'Click to configure URL'}
          </div>
        </div>
      )}

      {/* output handle */}
      <Handle
        type="source"
        position={Position.Right}
        id="response"
        style={{
          background: '#fff',
          border: '2px solid #3F51B5',
          width: 12,
          height: 12,
        }}
      />
    </div>
  );
};

export default HttpRequestNode;