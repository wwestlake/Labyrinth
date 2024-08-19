import React from 'react';
import { useNavigate } from 'react-router-dom';
import { signOut } from 'firebase/auth';
import { auth } from '../firebase-config';
import './Sidebar.css';

const Sidebar = ({ isCollapsed, toggleSidebar }) => {
  const navigate = useNavigate();

  const handleLogout = async () => {
    await signOut(auth);
    navigate('/');
  };

  return (
    <div className={`sidebar ${isCollapsed ? 'collapsed' : ''}`}>
      <div className="hamburger" onClick={toggleSidebar}>
        <span className="hamburger-icon">&#9776;</span>
      </div>
      <div className="menu-items">
        <div className="menu-item" onClick={() => navigate('/game-console/dashboard')}>
          <span className="icon">🏠</span>
          {!isCollapsed && <span className="text">Dashboard</span>}
        </div>
        <div className="menu-item" onClick={() => navigate('/game-console/current-room')}>
          <span className="icon">🗺️</span>
          {!isCollapsed && <span className="text">Current Room</span>}
        </div>
        <div className="menu-item" onClick={() => navigate('/game-console/chat')}>
          <span className="icon">💬</span>
          {!isCollapsed && <span className="text">Chat</span>}
        </div>
        <div className="menu-item" onClick={handleLogout}>
          <span className="icon">🚪</span>
          {!isCollapsed && <span className="text">Log Out</span>}
        </div>
      </div>
    </div>
  );
};

export default Sidebar;
