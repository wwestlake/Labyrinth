// src/components/Game.jsx
import React, { useState } from 'react';
import { useNavigate, NavLink, Routes, Route, Navigate } from 'react-router-dom';
import { logout } from '../services/authService';
import Home from './Home';  // Import the new Home component
import Room from './Room';
import Character from './Character';
import Items from './Items';
import Bank from './Bank';
import Quests from './Quests';
import ManagePlayers from './ManagePlayers';
import ReviewReports from './ReviewReports';
import ChatModeration from './ChatModeration';
import BanPlayers from './BanPlayers';
import GameConfiguration from './GameConfiguration';
import ContentManagement from './ContentManagement';
import EventScheduling from './EventScheduling';
import SupportTickets from './SupportTickets';
import PluginManager from './PluginManager';
import './Game.css';

const Game = () => {
  const [isPlayerSectionOpen, setIsPlayerSectionOpen] = useState(false);
  const [isModeratorSectionOpen, setIsModeratorSectionOpen] = useState(false);
  const [isStaffSectionOpen, setIsStaffSectionOpen] = useState(false);

  const navigate = useNavigate();

  const togglePlayerSection = () => setIsPlayerSectionOpen(!isPlayerSectionOpen);
  const toggleModeratorSection = () => setIsModeratorSectionOpen(!isModeratorSectionOpen);
  const toggleStaffSection = () => setIsStaffSectionOpen(!isStaffSectionOpen);

  const handleLogout = async () => {
    try {
      await logout();
      navigate('/');
    } catch (error) {
      console.error("Failed to logout:", error);
    }
  };

  return (
    <div className="game-container">
      <div className="sidebar">
        {/* Update the NavLink to point to the correct path for Home */}
        <NavLink to="/game/home" className={({ isActive }) => isActive ? 'menu-item active' : 'menu-item'}>Home</NavLink>
        <div className="menu-item" onClick={handleLogout}>Logout</div>
        {/* Highlight Options as active if the URL is '/game/options' */}
        <NavLink to="/game/options" className={({ isActive }) => isActive ? 'menu-item active' : 'menu-item'}>Options</NavLink>

        {/* Player Section */}
        <div className="menu-section">
          <div className="menu-item" onClick={togglePlayerSection}>
            Player {isPlayerSectionOpen ? '-' : '+'}
          </div>
          {isPlayerSectionOpen && (
            <div className="submenu">
              <NavLink to="/game/room" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Room</NavLink>
              <NavLink to="/game/character" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Character</NavLink>
              <NavLink to="/game/items" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Items</NavLink>
              <NavLink to="/game/bank" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Bank</NavLink>
              <NavLink to="/game/quests" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Quests</NavLink>
            </div>
          )}
        </div>

        {/* Moderator Section */}
        <div className="menu-section">
          <div className="menu-item" onClick={toggleModeratorSection}>
            Moderator {isModeratorSectionOpen ? '-' : '+'}
          </div>
          {isModeratorSectionOpen && (
            <div className="submenu">
              <NavLink to="/game/manage-players" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Manage Players</NavLink>
              <NavLink to="/game/review-reports" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Review Reports</NavLink>
              <NavLink to="/game/chat-moderation" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Chat Moderation</NavLink>
              <NavLink to="/game/ban-players" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Ban/Unban Players</NavLink>
            </div>
          )}
        </div>

        {/* Staff Section */}
        <div className="menu-section">
          <div className="menu-item" onClick={toggleStaffSection}>
            Staff {isStaffSectionOpen ? '-' : '+'}
          </div>
          {isStaffSectionOpen && (
            <div className="submenu">
              <NavLink to="/game/configuration" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Game Configuration</NavLink>
              <NavLink to="/game/content-management" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Content Management</NavLink>
              <NavLink to="/game/event-scheduling" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Event Scheduling</NavLink>
              <NavLink to="/game/support-tickets" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Support Tickets</NavLink>
              <NavLink to="/game/plugins" className={({ isActive }) => isActive ? 'submenu-item active' : 'submenu-item'}>Plugin Manager</NavLink> {/* Plugin Manager Link */}
            </div>
          )}
        </div>
      </div>

      <div className="content-area">
        <Routes>
          {/* Set the Home component as the default route */}
          <Route path="/" element={<Navigate to="/game/home" replace />} />
          <Route path="home" element={<Home />} />
          <Route path="room" element={<Room />} />
          <Route path="character" element={<Character />} />
          <Route path="items" element={<Items />} />
          <Route path="bank" element={<Bank />} />
          <Route path="quests" element={<Quests />} />
          <Route path="manage-players" element={<ManagePlayers />} />
          <Route path="review-reports" element={<ReviewReports />} />
          <Route path="chat-moderation" element={<ChatModeration />} />
          <Route path="ban-players" element={<BanPlayers />} />
          <Route path="configuration" element={<GameConfiguration />} />
          <Route path="content-management" element={<ContentManagement />} />
          <Route path="event-scheduling" element={<EventScheduling />} />
          <Route path="support-tickets" element={<SupportTickets />} />
          <Route path="plugins" element={<PluginManager />} /> {/* Route for Plugin Manager */}
        </Routes>
      </div>
    </div>
  );
};

export default Game;
