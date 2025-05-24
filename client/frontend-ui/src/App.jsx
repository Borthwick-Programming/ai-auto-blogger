import { useState, useEffect } from 'react'
import { WorkflowCanvas } from './components/WorkflowCanvas'
import './App.css'

function App() {
  const [projects, setProjects] = useState(null)
  const [error, setError]     = useState(null)
  const [active, setActive] = useState('');  // ← track the selected project

useEffect(() => {
    fetch('/api/projects')
      .then(res => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`)
        return res.json()
      })
      .then(data => setProjects(data))
      .catch(err => setError(err.message))
  }, [])

  if (error) return <div className="error">Error: {error}</div>
  if (projects === null) return <div>Loading projects…</div>
  if (projects.length === 0) return <div>No projects yet.</div>

  return (
    <div className="App">
      <h1>Your Workflow Projects</h1>
      <select value={active} onChange={e => setActive(e.target.value)}>
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
