import { nodeRegistry } from '../nodes';

/* Build the map ONCE – module-level, not per render */
export const nodeTypes = Object.fromEntries(
  Object.entries(nodeRegistry).map(([k, v]) => [k, v.Renderer]),
);

