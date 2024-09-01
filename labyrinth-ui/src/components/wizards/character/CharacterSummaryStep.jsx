// src/components/wizards/character/CharacterSummaryStep.jsx

import React from 'react';

const CharacterSummaryStep = ({ characterData, previousStep }) => {
  return (
    <div className="step-container">
      <h2>Character Summary</h2>
      <div className="form-group">
        <label>Name:</label>
        <p>{characterData.name}</p>
      </div>
      <div className="form-group">
        <label>Class:</label>
        <p>{characterData.characterClass}</p>
      </div>
      <div className="form-group">
        <label>Race:</label>
        <p>{characterData.race}</p>
      </div>
      {Object.entries(characterData.stats).map(([stat, value]) => (
        <div className="form-group" key={stat}>
          <label>{stat.charAt(0).toUpperCase() + stat.slice(1)}:</label>
          <p>{value}</p>
        </div>
      ))}
      <div className="button-group">
        <button onClick={previousStep}>Previous</button>
        <button onClick={() => alert('Character Created!')}>Finish</button>
      </div>
    </div>
  );
};

export default CharacterSummaryStep;
