import React from 'react';
import { Item } from '../types/ipc';

interface Props {
  items: Item[];
  onEdit: (item: Item) => void;
  onDelete: (id: number) => void;
}

export default function ItemList({ items, onEdit, onDelete }: Props) {
  return (
    <ul className="space-y-2">
      {items.map(item => (
        <li
          key={item.id}
          className="flex justify-between items-center border rounded px-4 py-2"
        >
          <span>{item.name}</span>
          <div className="flex gap-2">
            <button
              onClick={() => onEdit(item)}
              className="text-blue-600 hover:underline"
            >
              Edit
            </button>
            <button
              onClick={() => onDelete(item.id)}
              className="text-red-600 hover:underline"
            >
              Delete
            </button>
          </div>
        </li>
      ))}
    </ul>
  );
}
