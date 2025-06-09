// src/api/nodeInstancesApi.ts
import { API_BASE } from './httpBase';

import type {
  CreateNodeInstanceRequest,
  UpdateNodeInstanceRequest,
  NodeInstanceDto,             
} from './nodeTypesApi';



const base = `${API_BASE}/api/projects`;  // we’ll append {projectId}/nodeinstances

/** Generic helper copied from nodeTypesApi.ts */
const json = <T,>(r: Response) => r.json() as Promise<T>;

/* ---------- CRUD wrappers ---------- */

/** POST /api/projects/{projectId}/nodeinstances */
export const createNodeInstance = (
  projectId: string,
  dto: CreateNodeInstanceRequest,
) =>
  fetch(`${base}/${projectId}/nodeinstances`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  }).then(r => json<NodeInstanceDto>(r));

/** PUT /api/projects/{projectId}/nodeinstances/{nodeId} */
export const updateNodeInstance = async (
  projectId: string,
  nodeId: string,
  dto: UpdateNodeInstanceRequest
) => {
  const res = await fetch(`${API_BASE}/api/projects/${projectId}/nodeinstances/${nodeId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });

  // Throw if we got a non-2xx status, .catch will fire:
  if (!res.ok) {
    const errBody = await res.json().catch(() => ({}));
    throw new Error(`API 400: ${JSON.stringify(errBody)}`);
  }

  // If your API returns the updated node, you can still parse here:
  return res.json();
};

/** DELETE /api/projects/{projectId}/nodeinstances/{nodeId} */
export const deleteNodeInstance = (projectId: string, nodeId: string) =>
  fetch(`${base}/${projectId}/nodeinstances/${nodeId}`, { method: 'DELETE' });

export type { UpdateNodeInstanceRequest };
