import React from 'react';
import { useState, useEffect } from 'react';
import { Item } from '../types/ipc';

interface Props {
  onSubmit: (name: string, id?: number) => void;
  editingItem?: Item | null;
}

export default function ItemForm({ onSubmit, editingItem }: Props) {
  const [name, setName] = useState('');

  useEffect(() => {
    setName(editingItem?.name ?? '');
  }, [editingItem]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (name.trim()) onSubmit(name, editingItem?.id);
    setName('');
  };

  return (
    <form onSubmit={handleSubmit} className="flex gap-2 mb-4">
      <input
        type="text"
        placeholder="Item name"
        value={name}
        onChange={e => setName(e.target.value)}
        className="border rounded px-3 py-2 flex-1"
      />
      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        {editingItem ? 'Update' : 'Add'}
      </button>
    </form>
  );
}
