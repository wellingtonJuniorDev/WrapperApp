import React, { useEffect, useState } from 'react';
import ItemForm from '../components/ItemForm';
import ItemList from '../components/ItemList';
import { IPCAction, Item } from '../types/ipc';
import { sendIpcCommand } from '../communication/ipc';
import { Persistence } from '../communication/edge';

export default function Home() {
  const [items, setItems] = useState<Item[]>([]);
  const [editing, setEditing] = useState<Item | null>(null);
  const [useDll, setUseDll] = useState<boolean>(true);

  const fetchItems = async () => {
    const result = useDll
      ? await Persistence.listAll()
      : await sendIpcCommand({ action: IPCAction.LIST });

    setItems(result);
  };

  const addOrUpdateItem = async (name: string, id?: number) => {
    if (id !== undefined) {
      if (useDll) {
        await Persistence.update(id, name);
      } else {
        await sendIpcCommand({ action: IPCAction.UPDATE, id, name });
      }
    } else {
      if (useDll) {
        await Persistence.add(name);
      } else {
        await sendIpcCommand({ action: IPCAction.ADD, name });
      }
    }

    setEditing(null);
    await fetchItems();
  };

  const deleteItem = async (id: number) => {
    if (useDll) {
      await Persistence.remove(id);
    } else {
      await sendIpcCommand({ action: IPCAction.REMOVE, id });
    }

    await fetchItems();
  };

  const handleEdit = (item: Item) => setEditing(item);

  useEffect(() => {
    fetchItems();
  }, [useDll]);

  return (
    <div className="max-w-xl mx-auto mt-10">
      <div className="flex items-center justify-between mb-4">
        <h1 className="text-2xl font-semibold">Item Manager</h1>
        <label className="flex items-center space-x-2 text-sm">
          <input
            type="checkbox"
            checked={useDll}
            onChange={() => setUseDll(!useDll)}
          />
          <span>{useDll? 'Using DLL (electron-edge-js)' : 'Using IPC connection'}</span>
        </label>
      </div>

      <ItemForm onSubmit={addOrUpdateItem} editingItem={editing} />
      <ItemList items={items} onEdit={handleEdit} onDelete={deleteItem} />
    </div>
  );
}
