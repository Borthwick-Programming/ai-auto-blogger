// vite.config.js
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  base: '/',
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5015', // your ASP-NET backend
        changeOrigin: true,              // sets Host header = 5015
        secure: false,                   // ignore self-signed certs (HTTP is fine)
      },
    },
  },
  build: { outDir: 'dist' },
})
