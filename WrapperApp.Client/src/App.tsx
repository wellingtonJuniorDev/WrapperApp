import React from 'react';
import { createRoot } from 'react-dom/client';
import List from './pages/List';
import './index.css';

createRoot(document.getElementById("root")!)
  .render(<List />);
