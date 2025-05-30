import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { THEME_KEY } from './utils/prefs';
import './index.css'
import App from './App.jsx'
document.documentElement.dataset.theme = localStorage.getItem(THEME_KEY) || 'light';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
