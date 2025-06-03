// src/nodes/index.tsx
import type { NodeDefinition } from '../types/node';

import HttpRequestNode       from './httpRequest/HttpRequestNode';
import ConditionalBranchNode from './conditionalBranch/ConditionalBranchNode';

/**
 * Central registry that the canvas and config-drawer look up.
 */
export const nodeRegistry: Record<string, NodeDefinition> = {
  'http-request': {
    id:   'http-request',
    name: 'HTTP Request',
    Renderer:   HttpRequestNode,
    ConfigForm: () => <p>No UI yet</p>,       // stub
  },

  'conditional-branch': {
    id:   'conditional-branch',
    name: 'Conditional Branch',
    Renderer:   ConditionalBranchNode,
    ConfigForm: () => <p>No UI yet</p>,       // stub
  },
};
