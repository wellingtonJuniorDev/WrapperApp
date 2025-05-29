import { ipcMain } from 'electron';
import net from 'net';

const PIPE_NAME = '\\\\.\\pipe\\WrapperApp.Business.Pipe';

ipcMain.handle('wrapper:send', async (_event, command) => {
  return new Promise((resolve, reject) => {
    const client = net.createConnection(PIPE_NAME, () => {
      client.write(JSON.stringify(command) + '\n');
    });

    let data = '';
    client.on('data', chunk => data += chunk.toString());
    client.on('end', () => {
      try {
        resolve(JSON.parse(data));
      } catch (e) {
        reject(e);
      }
    });

    client.on('error', reject);
  });
});
