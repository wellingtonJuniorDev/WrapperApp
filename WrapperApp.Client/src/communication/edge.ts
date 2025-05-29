export const Persistence = {
  add: (name: string) => window.persistence.call('add', { name }),
  get: (id: number) => window.persistence.call('get', { id }),
  remove: (id: number) => window.persistence.call('remove', { id }),
  getCount: () => window.persistence.call('getCount', {}),
  update: (id: number, name: string) => window.persistence.call('update', { id, name }),
  listAll: () => window.persistence.call('listAll', {})
};