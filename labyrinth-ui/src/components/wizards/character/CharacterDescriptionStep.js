import React from 'react';

const CharacterDescriptionStep = ({ characterData, handleInputChange }) => {
  return (
    <div className="step-container">
      <h2>Step 3: Character Description</h2>
      <label>
        Description:
        <textarea
          name="description"
          value={characterData.description}
          onChange={handleInputChange}
          required
        />
      </label>
    </div>
  );
};

export default CharacterDescriptionStep;
