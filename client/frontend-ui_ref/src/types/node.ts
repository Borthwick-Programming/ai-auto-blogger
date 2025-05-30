/* src/types/node.ts */
import { JSONSchema7 } from 'json-schema';
import { NodeProps as FlowNodeProps } from 'reactflow';   // change path if using v9
import { Port } from './port';

/** Props received by any node renderer */
export type NodeRendererProps = FlowNodeProps;

/** Props received by a configuration form component */
export interface ConfigProps {
  /** Node-instance ID so the form can persist changes */
  nodeId: string;
  /** Current configuration as JSON string (exact shape per node) */
  configurationJson: string;
  /** Callback when the user edits the config */
  onChange: (json: string) => void;
}

export interface NodeDefinition {
  /** Registry key, e.g. "http-request" */
  id: string;
  /** Display name shown in UI */
  name: string;
  /** Optional JSON-Schema describing configuration fields */
  configurationSchema?: JSONSchema7;
  /** Ports exposed by the node */
  inputs?: Port[];
  outputs?: Port[];
  /** Icon + theme colour for the canvas */
  visual?: { icon: string; color: string };
  /** React-Flow renderer */
  Renderer: React.FC<NodeRendererProps>;
  /** React component shown in the side-drawer for config */
  ConfigForm: React.FC<ConfigProps>;
}
