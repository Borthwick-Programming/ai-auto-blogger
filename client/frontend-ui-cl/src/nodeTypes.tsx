// src/nodeTypes.ts
import type { NodeTypes, EdgeTypes } from '@xyflow/react';

import HttpRequestNode     from './components/nodes/HttpRequestNode';
import ConditionalBranchNode from './components/nodes/ConditionalBranchNode';

export const NODE_TYPES: NodeTypes = Object.freeze({
  'http-request':       HttpRequestNode,
  'conditional-branch': ConditionalBranchNode,
});

export const EDGE_TYPES: EdgeTypes = Object.freeze({});
