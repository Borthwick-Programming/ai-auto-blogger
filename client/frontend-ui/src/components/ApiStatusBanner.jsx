import React from 'react';
import './ApiStatusBanner.css';

export default function ApiStatusBanner({ online }) {
  if (online) return null;
  return (
    <div className="api-banner">
      <span>⚠️  Backend API is offline – retrying&nbsp;…</span>
    </div>
  );
}