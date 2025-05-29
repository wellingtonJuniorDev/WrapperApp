declare global {
  interface Window {
    api: {
      sendPersistenceCommand: (command: any) => Promise<any>;
    },
     persistence: {
      call: (method: string, args: any) => Promise<any>;
    }
  }
}

export {};