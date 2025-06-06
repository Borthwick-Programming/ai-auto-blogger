// src/api/httpBase.ts – tiny utility
export const API_BASE: string =
  import.meta.env.VITE_API_BASE?.toString().replace(/\/$/, '') ||
  'https://localhost:7201';
