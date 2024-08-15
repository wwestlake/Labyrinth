import React, { useEffect, useState } from 'react';
import CarouselComponent from './CarouselComponent';
import CharacterSheet from './CharacterSheet';
import Sidebar from './Sidebar';  // Ensure Sidebar is imported
import './Dashboard.css';  // Assuming you have some styles for the dashboard layout

const Dashboard = () => {
  const [characters, setCharacters] = useState([]);

  useEffect(() => {
    // Fetch character data from your API or database
    const fetchedCharacters = [
      { name: 'Hero', level: 10, class: 'Warrior', health: 100 },
      { name: 'Mage', level: 8, class: 'Wizard', health: 80 },
      { name: 'Rogue', level: 7, class: 'Thief', health: 70 },
      // Add more characters as needed
    ];
    setCharacters(fetchedCharacters);
  }, []);

 return (
  <div className="dashboard-container">
    <Sidebar /> {/* Include Sidebar */}
    <div className="dashboard-content">
      <h2>Your Characters</h2>
      <div className="carousel-container">
        <CarouselComponent
          items={characters.map((character, index) => (
            <CharacterSheet key={index} character={character} />
          ))}
        />
      </div>
    </div>
  </div>
);
 
};

export default Dashboard;
