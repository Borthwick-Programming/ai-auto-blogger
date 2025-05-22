import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [projects, setProjects] = useState(null)
  const [error, setError]     = useState(null)

  useEffect(() => {
    fetch('http://localhost:5015/api/projects', { credentials: 'include' })
    //fetch('/api/projects')
    //fetch('http://localhost:5015/api/projects')
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
      <ul>
        {projects.map(p => (
          <li key={p.id}>
            {p.name} <small>({p.id})</small>
          </li>
        ))}
      </ul>
    </div>
  )
}

export default App
