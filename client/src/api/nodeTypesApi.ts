// src/api/nodeTypesApi.ts

import { API_BASE } from './httpBase';


export interface NodeDefinition {
  id: string;
  name: string;
  description: string;
  reactComponent: string;
  icon?: string;
}

export interface CreateNodeInstanceRequest {
  nodeTypeId: string;
  configurationJson: string;
  positionX: number;
  positionY: number;
}

export interface UpdateNodeInstanceRequest {
  id: string,
  nodeTypeId:  string;
  configurationJson?: string;
  positionX?: number;
  positionY?: number;
}

export interface NodeInstanceDto extends CreateNodeInstanceRequest {
  id: string;
}

const base = `${API_BASE}/api/nodes`;

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
