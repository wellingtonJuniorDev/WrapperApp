import { ipcMain } from 'electron';
import edge from 'electron-edge-js';
import path from 'path';

const dllName = 'WrapperApp.Library';
const dllPath = path.join(
  "assets",
  dllName + '.dll'
);
console.log(`Using DLL at: ${dllPath}`);

const edgeFunctions = {
  add: edge.func({ assemblyFile: dllPath, typeName: `${dllName}.Adapters.EdgePersistenceAdapter`, methodName: 'Add' }),
  get: edge.func({ assemblyFile: dllPath, typeName: `${dllName}.Adapters.EdgePersistenceAdapter`, methodName: 'Get' }),
  remove: edge.func({ assemblyFile: dllPath, typeName: `${dllName}.Adapters.EdgePersistenceAdapter`, methodName: 'Remove' }),
  getCount: edge.func({ assemblyFile: dllPath, typeName: `${dllName}.Adapters.EdgePersistenceAdapter`, methodName: 'GetCount' }),
  update: edge.func({ assemblyFile: dllPath, typeName: `${dllName}.Adapters.EdgePersistenceAdapter`, methodName: 'Update' }),
  listAll: edge.func({ assemblyFile: dllPath, typeName: `${dllName}.Adapters.EdgePersistenceAdapter`, methodName: 'ListAll' }),
};

function call(method: keyof typeof edgeFunctions, args: any): Promise<any> {
  return new Promise((resolve, reject) => {
    edgeFunctions[method](args, (error: any, result: any) => {
      if (error) reject(error);
      else resolve(result);
    });
  });
}

ipcMain.handle('persistence:call', async (_event, method: string, args: any) => {
  return await call(method as any, args);
});
