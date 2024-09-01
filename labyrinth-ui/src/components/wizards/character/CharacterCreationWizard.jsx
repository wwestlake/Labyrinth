// src/components/wizards/character/CharacterCreationWizard.jsx

import React, { useState } from 'react';
import StepWizard from 'react-step-wizard';
import BasicInfoStep from './BasicInfoStep';
import StatsAllocationStep from './StatsAllocationStep';
import CharacterSummaryStep from './CharacterSummaryStep';
import './CharacterCreationWizard.css';

const CharacterCreationWizard = () => {
  const [characterData, setCharacterData] = useState({
    name: '',
    characterClass: '',
    race: '',
    stats: {
      strength: 0,
      dexterity: 0,
      intelligence: 0,
      wisdom: 0,
      charisma: 0,
      constitution: 0,
    },
  });

  const updateCharacterData = (newData) => {
    setCharacterData((prevData) => ({ ...prevData, ...newData }));
  };

  return (
    <div className="character-creation-wizard">
      <StepWizard>
        <BasicInfoStep characterData={characterData} updateCharacterData={updateCharacterData} />
        <StatsAllocationStep characterData={characterData} updateCharacterData={updateCharacterData} />
        <CharacterSummaryStep characterData={characterData} />
      </StepWizard>
    </div>
  );
};

export default CharacterCreationWizard;
