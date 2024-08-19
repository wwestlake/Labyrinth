import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './index.css';
import Login from './components/Login';
import Signup from './components/Signup';
import PrivateRoute from './components/PrivateRoute';
import OpeningPage from './components/OpeningPage';
import GameConsole from './components/GameConsole';
import 'bootstrap/dist/css/bootstrap.min.css';
import Dashboard from './components/Dashboard';
import Room from './components/Room';
import ChatBox from './components/ChatBox';

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <Router>
      <Routes>
        <Route path="/" element={<OpeningPage />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />
        <Route path="/game-console/*" element={
          <PrivateRoute>
            <GameConsole />
          </PrivateRoute>
        }> 
          <Route path="dashboard" element={<Dashboard />} />
          <Route path="current-room" element={<Room />} />
          <Route path="chat" element={<ChatBox />} />
        </Route>
      </Routes>
    </Router>
  </React.StrictMode>
);
