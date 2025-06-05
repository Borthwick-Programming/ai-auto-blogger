// src/components/nodeRegistry.ts
import { ConditionalBranchNode, HttpRequestNode } from './nodes'; // barrel re-exports exist already :contentReference[oaicite:1]{index=1}

export const NODE_TYPES = {
  ConditionalBranch: ConditionalBranchNode,
  HttpRequest:       HttpRequestNode,
  // Add new nodes here
};

export const EDGE_TYPES = {};  // add custom edge renderers later
