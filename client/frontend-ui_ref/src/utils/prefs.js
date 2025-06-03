export const THEME_KEY   = 'wf-theme';
export const PROJECT_KEY = 'wf-projectId';
export const getPref  = k => localStorage.getItem(k);
export const setPref  = (k,v) => localStorage.setItem(k,v);
