import { API_BASE } from './httpBase';

export interface PrePrompt {
  id: number;
  name: string;
  promptText: string;
}

export const listPrePrompts = async(): Promise<PrePrompt[]> =>
  (await fetch(`${API_BASE}/api/preprompts`)).json();

export const createPrePrompt = async(p: Omit<PrePrompt, 'id'>) =>
  (await fetch(`${API_BASE}/api/preprompts`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(p),
  })).json();

export const updatePrePrompt = async(p: PrePrompt) =>
  fetch(`${API_BASE}/api/preprompts/${p.id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(p),
  });

export const deletePrePrompt = async(id: number) =>
  fetch(`${API_BASE}/api/preprompts/${id}`, { method: 'DELETE' });
