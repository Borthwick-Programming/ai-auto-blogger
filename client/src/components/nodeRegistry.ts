// src/components/nodeRegistry.ts
import { ConditionalBranchNode, HttpRequestNode } from './nodes'; // barrel re-exports exist already :contentReference[oaicite:1]{index=1}
import AffiliateLinkInputNode from './nodes/AffiliateLinkInput/AffiliateLinkInputNode';

export const NODE_TYPES = {
  ConditionalBranch: ConditionalBranchNode,
  HttpRequest:       HttpRequestNode,
  affiliateLinkInput: AffiliateLinkInputNode
  // Add new nodes here
};

export const EDGE_TYPES = {};  // add custom edge renderers later
