// src/api/nodeTypesApi.ts

export interface NodeDefinition {
  id: string;
  name: string;
  description: string;
  reactComponent: string;
  icon?: string;
}

const base = '/api/nodes';

// generic helper to keep code DRY
const json = <T,>(r: Response) => r.json() as Promise<T>;

export const getAllNodeTypes = () => fetch(base).then(r => json<NodeDefinition[]>(r));
export const getNodeType     = (id: string) =>
  fetch(`${base}/${id}`).then(r => json<NodeDefinition>(r));

export const createNodeType  = (dto: NodeDefinition) =>
  fetch(base, { method: 'POST', body: JSON.stringify(dto), headers: {'Content-Type':'application/json'} });

export const updateNodeType  = (dto: NodeDefinition) =>
  fetch(`${base}/${dto.id}`, { method: 'PUT', body: JSON.stringify(dto), headers: {'Content-Type':'application/json'} });

export const deleteNodeType  = (id: string) =>
  fetch(`${base}/${id}`, { method: 'DELETE' });
