// See the Electron documentation for details on how to use preload scripts:
// https://www.electronjs.org/docs/latest/tutorial/process-model#preload-scripts

import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('api', {
  sendPersistenceCommand: (command: any) =>
    ipcRenderer.invoke('wrapper:send', command)
});

contextBridge.exposeInMainWorld('persistence', {
  call: (method: string, args: any) => ipcRenderer.invoke('persistence:call', method, args)
});