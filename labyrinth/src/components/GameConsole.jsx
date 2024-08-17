import React, { useState } from 'react';
import Sidebar from './Sidebar';
import Dashboard from './Dashboard'; 
import './GameConsole.css';
import { Outlet } from 'react-router-dom';

const GameConsole = () => {
  const [isCollapsed, setIsCollapsed] = useState(false);

  const toggleSidebar = () => {
    setIsCollapsed(!isCollapsed);
  };

  return (
    <div className="game-console">
      <Sidebar isCollapsed={isCollapsed} toggleSidebar={toggleSidebar} />
      <div className={`content-area ${isCollapsed ? 'expanded' : ''}`}>
      <Outlet /> {/* This will render the component based on the current route */}
      </div>
    </div>
  );
};

export default GameConsole;
