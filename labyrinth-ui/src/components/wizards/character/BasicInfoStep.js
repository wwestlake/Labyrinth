// src/components/wizards/character/BasicInfoStep.jsx

import React, { useState, useEffect } from 'react';
import axios from 'axios';

const BasicInfoStep = ({ characterData, updateCharacterData, nextStep }) => {
  const [isNameUnique, setIsNameUnique] = useState(true);
  const [isNextEnabled, setIsNextEnabled] = useState(false);

  const handleInputChange = async (e) => {
    const { name, value } = e.target;
    updateCharacterData({ [name]: value });

    // Check if all fields are filled
    const allFieldsFilled = characterData.name && characterData.characterClass && characterData.race;
    setIsNextEnabled(allFieldsFilled && isNameUnique);

    // Check for name uniqueness
    if (name === 'name' && value) {
      try {
        const response = await axios.get(`/api/character/check-name-unique?name=${value}`);
        setIsNameUnique(response.data.isUnique);
        setIsNextEnabled(allFieldsFilled && response.data.isUnique);
      } catch (error) {
        console.error('Error checking name uniqueness:', error);
        setIsNameUnique(false);
      }
    }
  };

  return (
    <div className="step-container">
      <h2>Basic Information</h2>
      <div className="form-group">
        <label htmlFor="name">Name:</label>
        <input
          type="text"
          id="name"
          name="name"
          value={characterData.name}
          onChange={handleInputChange}
        />
        {!isNameUnique && <p className="error-message">This name is already taken. Please choose another one.</p>}
      </div>
      <div className="form-group">
        <label htmlFor="characterClass">Class:</label>
        <input
          type="text"
          id="characterClass"
          name="characterClass"
          value={characterData.characterClass}
          onChange={handleInputChange}
        />
      </div>
      <div className="form-group">
        <label htmlFor="race">Race:</label>
        <input
          type="text"
          id="race"
          name="race"
          value={characterData.race}
          onChange={handleInputChange}
        />
      </div>
      <div className="button-group">
        <button onClick={nextStep} disabled={!isNextEnabled}>Next</button>
      </div>
    </div>
  );
};

export default BasicInfoStep;
