import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config
export default defineConfig({
    plugins: [react()],
    build: {
      //target: 'chrome105',
      rollupOptions: {
        external: ['electron-edge-js'],
      },
    },
});
