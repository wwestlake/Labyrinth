// src/components/Home.jsx
import React from 'react';
import ChatClient from './ChatClient';
import './Home.css'; // Ensure this file exists with the styles

const Home = () => {
  return (
    <div className="home-container">
      <div className="grid">
        <div className="grid-item upper-left">
          <h2>Upper Left Section</h2>
          <p>Content for the upper left section.</p>
        </div>
        <div className="grid-item upper-right">
          <ChatClient />
        </div>
        <div className="grid-item lower-left">
          <h2>Lower Left Section</h2>
          <p>Content for the lower left section.</p>
        </div>
        <div className="grid-item lower-right">
          <h2>Lower Right Section</h2>
          <p>Content for the lower right section.</p>
        </div>
      </div>
    </div>
  );
};

export default Home;
