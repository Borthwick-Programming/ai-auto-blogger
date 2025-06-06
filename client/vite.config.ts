import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Anything starting with /api → http://localhost:5015
      '/api': {
        target: 'http://localhost:5015',
        changeOrigin: true,
        secure: false,
        rewrite: path => path,          // keep /api prefix
      },
    },
  },
});
