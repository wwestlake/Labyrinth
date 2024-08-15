import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { signOut } from 'firebase/auth';
import { auth } from '../firebase-config';
import './Sidebar.css'; // We'll add styles here

const Sidebar = () => {
  const [isCollapsed, setIsCollapsed] = useState(true);
  const navigate = useNavigate();

  const toggleSidebar = () => {
    setIsCollapsed(!isCollapsed);
  };

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
        <div className="menu-item" onClick={() => navigate('/dashboard')}>
          <span className="icon">🏠</span>
          {isCollapsed && <span className="text">Dashboard</span>}
        </div>
        {/* Add more menu items here */}
        <div className="menu-item" onClick={handleLogout}>
          <span className="icon">🚪</span>
          {isCollapsed && <span className="text">Log Out</span>}
        </div>
      </div>
    </div>
  );
};

export default Sidebar;
