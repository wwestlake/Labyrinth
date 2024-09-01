// src/components/Room.jsx
import React, { useState } from 'react';
import CharacterCreationWizard from './wizards/character/CharacterCreationWizard';

const Character = () => {
  const [state, setState] = useState({
    // Define any initial state for the Room component here
  });

  return (
    <div>
      <CharacterCreationWizard />
    </div>
  );
};

export default Character;

