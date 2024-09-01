import React from 'react';

const ReviewConfirmStep = ({ characterData, handleCharacterCreation }) => {
  return (
    <div className="step-container">
      <h2>Step 4: Review & Confirm</h2>
      <div>
        <p><strong>Name:</strong> {characterData.name}</p>
        <p><strong>Class:</strong> {characterData.characterClass}</p>
        <p><strong>Race:</strong> {characterData.race}</p>
        <p><strong>Description:</strong> {characterData.description}</p>
        {/* Display allocated stats */}
        <button onClick={handleCharacterCreation}>Create Character</button>
      </div>
    </div>
  );
};

export default ReviewConfirmStep;
