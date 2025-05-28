const base = '/api';
const safeFetch = (url, opts) =>
  fetch(url, opts)
    .then(r => {
      if (!r.ok) throw new Error(`HTTP ${r.status}`);
      return r.json();                  //  <- happy path
    })
    .catch(err => {
      console.warn('[API offline]', err.message);
      return null;                      // caller must handle null
    });

export const getProjects         = () => fetch(`${base}/projects`).then(r => r.json());
export const getNodeDefinitions  = () => fetch(`${base}/nodes`).then(r => r.json());
export const getInstances        = pid => fetch(`${base}/projects/${pid}/nodeinstances`).then(r => r.json());
export const getConnections      = pid => fetch(`${base}/projects/${pid}/nodeconnections`).then(r => r.json());

// <<save-instance-pos>>
export const saveInstancePos = (pid, id,nodeTypeId, pos) =>
  safeFetch(`/api/projects/${pid}/nodeinstances/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      id,
      nodeTypeId,
      positionX: pos.x,   
      positionY: pos.y
    }),
  });
