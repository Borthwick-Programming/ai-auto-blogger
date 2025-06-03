const base = '/api';
const safeFetch = (url, opts) =>
  fetch(url, opts)
    .then(r => {
      if (!r.ok) throw new Error(`HTTP ${r.status}`);
       return r.status === 204 ? null : r.json();
    })
    .catch(err => {
      console.warn('[API offline]', err.message);
      return null;                      // caller must handle null
    });

export const getProjects         = () => fetch(`${base}/projects`).then(r => r.json());
export const getNodeDefinitions  = () => fetch(`${base}/nodes`).then(r => r.json());
export const getInstances        = pid => fetch(`${base}/projects/${pid}/nodeinstances`).then(r => r.json());
export const getConnections      = pid => fetch(`${base}/projects/${pid}/nodeconnections`).then(r => r.json());
export const createNode = (pid, dto) =>
  safeFetch(
    `/api/projects/${pid}/nodeinstances`,
     { method:'POST', headers:{'Content-Type':'application/json'}, body: JSON.stringify(dto) }
    );
 export const deleteNode = (pid, nid) => safeFetch(
  `/api/projects/${pid}/nodeinstances/${nid}`,
   { method:'DELETE' }
  );

// <<save-instance-pos>>
export const saveInstancePos = (
  pid,
   id,
   nodeTypeId,
   pos,
  configurationJson) =>
  safeFetch(`/api/projects/${pid}/nodeinstances/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      id,
      nodeTypeId,
      configurationJson:
        typeof configurationJson === 'string'
          ? configurationJson
          : JSON.stringify(configurationJson ?? {}),
      positionX: pos.x,   
      positionY: pos.y
    }),
  });

  /* ------------------------------------------------------------------
   Update an entire NodeInstance (position, config or label)
   ------------------------------------------------------------------ */
// <<update-node>>
  export const updateNodeInstance = (pid, node) =>
  safeFetch(`/api/projects/${pid}/nodeinstances/${node.id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      id:         node.id,
      nodeTypeId: node.nodeTypeId,
      configurationJson: node.data.configurationJson,
      positionX:  node.position.x,
      positionY:  node.position.y,
    }),
  });
