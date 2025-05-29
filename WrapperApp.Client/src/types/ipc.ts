export interface Item {
  id: number;
  name: string;
}

export interface IPCRequest {
  action: IPCAction;
  id?: number;
  name?: string;
}

export enum IPCAction {
  ADD = 'add',
  UPDATE = 'update',
  REMOVE = 'remove',
  LIST = 'list',
  GET = 'get',
  COUNT = 'count'
}