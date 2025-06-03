import React, { useEffect, useState } from 'react';
import Canvas from '../components/Canvas';
import * as api from '../api/workflowengine';

import useApiStatus      from '../hooks/useApiStatus';
import { THEME_KEY, PROJECT_KEY, setPref, getPref } from '../utils/prefs';
import './WorkflowPage.css';     // layout styles
import ApiStatusBanner   from '../components/ApiStatusBanner';

export default function WorkflowPage() {
  const { online, trying, attempt, currentMs, restart } = useApiStatus();
  const [projects, setProjects]   = useState([]);
  const [projectId, setProjectId] = useState(getPref(PROJECT_KEY) || '');

  /* load once */
  useEffect(() => {
    api.getProjects().then(setProjects);
  }, []);

  // <<toggle-theme>>
const toggleTheme = () => {
  const root = document.documentElement;
          const current = root.dataset.theme; 
          root.dataset.theme =  current === 'light' ? 'dark' : 'light';
          setPref(THEME_KEY, root.dataset.theme);
};

  return (
    <div className="wf-page">
      <ApiStatusBanner online={online} trying={trying} attempt={attempt} currentMs={currentMs}/> 
      <header className="wf-header">
  {online && (                       /* ← conditional wrapper */
    <>
      <select
        aria-label="Project selector"
        value={projectId}
        //onChange={e => setProjectId(e.target.value)}
        onChange={e => {
          const val = e.target.value;
          setProjectId(val);
          setPref(PROJECT_KEY, val);}}
      >
        <option value="">— pick a project —</option>
        {projects.map(p => (
          <option key={p.id} value={p.id}>{p.name}</option>
        ))}
      </select>

      <button
        className="theme-btn"
        type="button"
        aria-label="Toggle light/dark theme"
        onClick={toggleTheme}
      />
    </>
  )}
</header>

      <main className="wf-canvas">
        {
        online ? 
        (
          projectId ? <Canvas projectId={projectId} />
          : 
          <p className="wf-hint">Select a project to load its workflow.</p>
        ) 
          : 
          trying ? 
        (
          <p className="wf-hint">Waiting for backend…</p>
        ) 
          : 
        (
          <div className="wf-hint">
          <p>Backend is still offline.</p>
          <button onClick={restart}>Retry</button>
          </div>
        )
      }
      </main>
    </div>
  );
}
