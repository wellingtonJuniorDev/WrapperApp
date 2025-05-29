export const sendIpcCommand = async (command: any) => {
  return await window.api.sendPersistenceCommand(command);
};