// src/components/wizards/character/StatsAllocationStep.jsx

import React, { useState, useEffect } from 'react';

const StatsAllocationStep = ({ characterData, updateCharacterData, nextStep, previousStep }) => {
  const [availablePoints, setAvailablePoints] = useState(10); // Example initial points, adjust as needed
  const [isNextEnabled, setIsNextEnabled] = useState(false);

  const handleStatChange = (stat, delta) => {
    if ((availablePoints > 0 && delta > 0) || (characterData.stats[stat] > 0 && delta < 0)) {
      const newStatValue = characterData.stats[stat] + delta;
      updateCharacterData({ stats: { ...characterData.stats, [stat]: newStatValue } });
      setAvailablePoints(availablePoints - delta);
    }
  };

  useEffect(() => {
    // Enable next button when all points are allocated
    setIsNextEnabled(availablePoints === 0);
  }, [availablePoints]);

  return (
    <div className="step-container">
      <h2>Allocate Stats</h2>
      <p>Available Points: {availablePoints}</p>
      {Object.keys(characterData.stats).map((stat) => (
        <div className="form-group" key={stat}>
          <label>{stat.charAt(0).toUpperCase() + stat.slice(1)}:</label>
          <div className="stat-controls">
            <button onClick={() => handleStatChange(stat, -1)} disabled={characterData.stats[stat] <= 0}>-</button>
            <input type="number" value={characterData.stats[stat]} readOnly />
            <button onClick={() => handleStatChange(stat, 1)} disabled={availablePoints <= 0}>+</button>
          </div>
        </div>
      ))}
      <div className="button-group">
        <button onClick={previousStep}>Previous</button>
        <button onClick={nextStep} disabled={!isNextEnabled}>Next</button>
      </div>
    </div>
  );
};

export default StatsAllocationStep;
