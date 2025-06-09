import React, { useState, useEffect } from 'react';
import WorkflowCanvas from './components/WorkflowCanvas.tsx';
import NodePalette from './components/NodePalette.tsx';
import './App.css';
import { API_BASE } from './api/httpBase';

interface Project {
  id: string;
  name: string;
}

function App() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [selectedProjectId, setSelectedProjectId] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  const apiBaseUrl = API_BASE;

  // Load projects on mount
  useEffect(() => {
    loadProjects();
  }, []);

  const loadProjects = async () => {
    try {
      setIsLoading(true);
      setError(null);
      
      const response = await fetch(`${apiBaseUrl}/api/projects`);
      if (!response.ok) {
        throw new Error('Failed to load projects');
      }
      
      const projectsData: Project[] = await response.json();
      setProjects(projectsData);
      
      // Auto-select first project if available
      if (projectsData.length > 0 && !selectedProjectId) {
        setSelectedProjectId(projectsData[0].id);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load projects');
    } finally {
      setIsLoading(false);
    }
  };

  const createNewProject = async () => {
    const projectName = prompt('Enter project name:');
    if (!projectName) return;

    try {
      const response = await fetch(`${apiBaseUrl}/api/projects`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: projectName }),
      });

      if (!response.ok) {
        throw new Error('Failed to create project');
      }

      const newProject: Project = await response.json();
      setProjects(prev => [...prev, newProject]);
      setSelectedProjectId(newProject.id);
    } catch (err) {
      alert('Failed to create project: ' + (err instanceof Error ? err.message : 'Unknown error'));
    }
  };

  if (isLoading) {
    return (
      <div className="app-loading">
        <h2>Loading...</h2>
        <p>Connecting to workflow engine...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="app-error">
        <h2>Connection Error</h2>
        <p>{error}</p>
        <p>Make sure your .NET Core API is running on {apiBaseUrl}</p>
        <button onClick={loadProjects}>Retry</button>
      </div>
    );
  }

  return (
    <div className="app"> {/* FLEX column root */}
      {/* ---------- HEADER ---------- */}
      <header className="app-header">
        <h1>🔄 Workflow Automation Builder</h1>
        <div className="project-controls">
          <select
            value={selectedProjectId || ''}
            onChange={(e) => setSelectedProjectId(e.target.value)}
            disabled={projects.length === 0}
          >
            {projects.length === 0 ? (
              <option value="">No projects available</option>
            ) : (
              <>
                <option value="">Select a project...</option>
                {projects.map(project => (
                  <option key={project.id} value={project.id}>
                    {project.name}
                  </option>
                ))}
              </>
            )}
          </select>
          <button onClick={createNewProject}>+ New Project</button>
        </div>
      </header>

      {/* ---------- MAIN WORK AREA ---------- */}
      <main className="app-main">
        <NodePalette />
        {selectedProjectId ? (
          <WorkflowCanvas 
            projectId={selectedProjectId} 
            apiBaseUrl={apiBaseUrl}
          />
        ) : (
          <div className="no-project-selected">
            <h2>Welcome to Workflow Builder!</h2>
            <p>Select a project from the dropdown above or create a new one to get started.</p>
            <div className="getting-started">
              <h3>What you can build:</h3>
              <ul>
                <li>🌐 <strong>Auto-blogging workflows</strong> - Fetch content, process it, and publish</li>
                <li>🎥 <strong>AI video generation</strong> - Create videos from text content</li>
                <li>📱 <strong>Social media automation</strong> - Post content across platforms</li>
                <li>🔗 <strong>Affiliate marketing funnels</strong> - Automate your marketing pipeline</li>
              </ul>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}

export default App;