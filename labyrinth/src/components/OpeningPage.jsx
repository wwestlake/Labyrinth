import React, { useState } from 'react';
import EmersiveGamePlay from '../assets/images/EmersiveGamePlay,.webp'
import RealTimeCombat from '../assets/images/RealTimeCombat.webp'
import EndlessExploration from '../assets/images/EndlessExploration.webp'
import { useNavigate } from 'react-router-dom';
import logo from '../assets/labyrinth-logo.webp';
import './OpeningPage.css';
import Signup from './Signup'; // Import the Signup component
import Login from './Login'; // Import the Login component

const OpeningPage = () => {
  const [showSignup, setShowSignup] = useState(false);
  const [showLogin, setShowLogin] = useState(false);
  const navigate = useNavigate();

  const handleSignupClick = () => {
    setShowSignup(true);
    setShowLogin(false); // Hide Login if Signup is clicked
  };

  const handleLoginClick = () => {
    setShowLogin(true);
    setShowSignup(false); // Hide Signup if Login is clicked
  };

  return (
    <div className="opening-page">
      {/* Header Section */}
      <header className="header">
        <div className="header-content">
          <img src={logo} alt="Labyrinth Logo" className="logo" />
          <div className="site-description">
            <h1>Labyrinth MUD</h1>
            <p>An epic dungeon crawler experience where every turn could be your last. Create a character, explore the maze, engage in combat, and uncover hidden secrets.</p>
          </div>
        </div>
      </header>

      {/* Sticky Buttons Section */}
      <div className="sticky-buttons">
        <div className="card" onClick={handleSignupClick}>
          <h2>Create Account</h2>
          <p>Sign up to start your adventure in the Labyrinth.</p>
        </div>
        <div className="card" onClick={handleLoginClick}>
          <h2>Join Game</h2>
          <p>Already have an account? Dive back into the adventure.</p>
        </div>
      </div>

      {/* Signup/Login Component Section */}
      <div className="expandable-section">
        {showSignup && <Signup />}
        {showLogin && <Login />}
      </div>

      {/* Feature Section */}
      <section className="features">
        <h2>Explore the Features</h2>
        <div className="feature">
        <img src={EmersiveGamePlay} alt="Immersive Dungeon Gameplay" />
          <div className="feature-text">
            <h3>Immersive Gameplay</h3>
            <p>Experience a deeply immersive world where every choice matters.</p>
          </div>
        </div>
        <div className="feature">
          <img src={RealTimeCombat} alt="Real Time Combat" />
          <div className="feature-text">
            <h3>Real-time Combat</h3>
            <p>Engage in thrilling real-time combat with enemies lurking in every corner.</p>
          </div>
        </div>
        <div className="feature">
          <img src={EndlessExploration} alt="Endless Exploration" />
          <div className="feature-text">
            <h3>Endless Exploration</h3>
            <p>Uncover the secrets of the Labyrinth through endless exploration.</p>
          </div>
        </div>
      </section>
    </div>
  );
};

export default OpeningPage;
