import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [projects, setProjects] = useState(null)
  const [error, setError]     = useState(null)

useEffect(() => {
  fetch('/api/projects')
    .then(res => res.json())
    .then(data => {
      console.log("(App.jsx)",data); 
      setProjects(data);
    });
}, []);

  if (error) return <div className="error">Error: {error}</div>
  if (projects === null) return <div>Loading projects…</div>
  if (projects.length === 0) return <div>No projects yet.</div>

  return (
    <div className="App">
      <h1>Your Workflow Projects</h1>
      <select onChange={e => setActive(e.target.value)}>
        <option value="">-- pick a project --</option>
        {projects.map(p => (
          <option key={p.id} value={p.id}>
            {p.name}
          </option>
        ))}
      </select>

      {active && <WorkflowCanvas projectId={active} />}
    </div>
  );
}

export default App
