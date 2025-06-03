import React, { useState } from 'react';
import {
  Handle,
  Position,
  useReactFlow,
  type Node,
  type NodeProps,
} from '@xyflow/react';

/* ---------- data you store in node.data ---------- */
interface ConditionalBranchNodeData extends Record<string, unknown> {
  configuration: {
    expression?: string;
  };
  nodeTypeId: string;
  nodeId: string;
}
/* full React-Flow node type */
type BranchRFNode = Node<ConditionalBranchNodeData>;
/* ------------------------------------------------- */

const ConditionalBranchNode: React.FC<NodeProps<BranchRFNode>> = ({
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
      className={`conditional-branch-node ${selected ? 'selected' : ''}`}
      style={{
        background: '#FF9800',
        color: 'white',
        border: selected ? '2px solid #F57C00' : '1px solid #FF9800',
        borderRadius: 8,
        padding: 12,
        minWidth: 200,
        boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
        position: 'relative',
      }}
    >
      {/* input handle */}
      <Handle
        type="target"
        position={Position.Left}
        id="input"
        style={{
          background: '#fff',
          border: '2px solid #FF9800',
          width: 12,
          height: 12,
        }}
      />

      {/* header */}
      <div style={{ display: 'flex', alignItems: 'center', marginBottom: 8 }}>
        <div style={{ marginRight: 8 }}>🔀</div>
        <strong>Conditional Branch</strong>
      </div>

      {/* body */}
      {isEditing ? (
        <div
          style={{
            background: 'rgba(255,255,255,0.1)',
            padding: 8,
            borderRadius: 4,
          }}
        >
          <label style={{ fontSize: 12 }}>Expression:</label>
          <textarea
            rows={3}
            value={tempConfig.expression || ''}
            onChange={e =>
              setTempConfig({ ...tempConfig, expression: e.target.value })
            }
            placeholder="input.status === 200"
            style={{
              width: '100%',
              resize: 'vertical',
              fontFamily: 'monospace',
              fontSize: 12,
              marginBottom: 8,
            }}
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
        <div
          onClick={() => setIsEditing(true)}
          style={{
            cursor: 'pointer',
            padding: 8,
            background: 'rgba(255,255,255,0.1)',
            borderRadius: 4,
            minHeight: 40,
          }}
        >
          <div style={{ fontSize: 12, opacity: 0.8 }}>Condition:</div>
          <div
            style={{
              fontFamily: 'monospace',
              wordBreak: 'break-word',
              fontSize: 14,
              minHeight: 20,
            }}
          >
            {data.configuration.expression || 'Click to configure expression'}
          </div>
        </div>
      )}

      {/* TRUE handle */}
      <Handle
        type="source"
        position={Position.Right}
        id="true"
        style={{
          background: '#4CAF50',
          border: '2px solid #4CAF50',
          width: 12,
          height: 12,
          top: '60%',
        }}
      />
      {/* FALSE handle */}
      <Handle
        type="source"
        position={Position.Right}
        id="false"
        style={{
          background: '#f44336',
          border: '2px solid #f44336',
          width: 12,
          height: 12,
          top: '80%',
        }}
      />

      {/* labels */}
      <div
        style={{
          position: 'absolute',
          right: -35,
          top: '55%',
          fontSize: 10,
          color: '#4CAF50',
          fontWeight: 'bold',
        }}
      >
        TRUE
      </div>
      <div
        style={{
          position: 'absolute',
          right: -40,
          top: '75%',
          fontSize: 10,
          color: '#f44336',
          fontWeight: 'bold',
        }}
      >
        FALSE
      </div>
    </div>
  );
};

export default ConditionalBranchNode;
