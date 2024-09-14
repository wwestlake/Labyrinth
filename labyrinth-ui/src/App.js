// src/App.js
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { GameProvider } from './context/GameContext';
import LandingPage from './components/LandingPage';
import PluginManager from './components/PluginManager';
import Game from './components/Game';
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
  return (
    <GameProvider>
      <Router>
        <div className="App">
          <Routes>
            <Route path="/" element={<LandingPage />} />
            <Route path="/game/*" element={<Game />} />
          </Routes>
        </div>
      </Router>
    </GameProvider>
  );
};

export default App;
