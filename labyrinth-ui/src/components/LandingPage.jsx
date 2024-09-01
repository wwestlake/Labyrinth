import React from 'react';
import DungeonCarousel from './DungeonCarousel'; // Adjust the path as needed
import SignInForm from './SignInForm';
import SignupForm from './SignupForm';
import './LandingPage.css'; // Import CSS if you have additional styles

const LandingPage = () => {
  return (
    <div className="landing-page">
      <DungeonCarousel />
      <div className="container mt-4">
        <div className="row">
          <div className="col-md-6">
            <div className="card">
              <div className="card-body">
                <SignInForm />
              </div>
            </div>
          </div>
          <div className="col-md-6">
            <div className="card">
              <div className="card-body">
                <SignupForm />
              </div>
            </div>
          </div>
        </div>
        <div className="mt-4">
          <h2>Welcome Message</h2>
          <p>And if there's anything you require, please don't hesitate to scream.</p>
          <p>Welcome to the dungeon...</p>
        </div>
      </div>
    </div>
  );
};

export default LandingPage;
