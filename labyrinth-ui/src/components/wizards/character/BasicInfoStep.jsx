import React, { useState, useEffect } from 'react';
import { checkCharacterNameUnique } from '../../../services/characterService';
import './BasicInfoStep.css';

const BasicInfoStep = ({ onChange, formData, isReadOnly, characterCreated }) => {
  const [characterName, setCharacterName] = useState(formData.name || '');
  const [race, setRace] = useState(formData.race || '');
  const [characterClass, setCharacterClass] = useState(formData.characterClass || '');
  const [isNameUnique, setIsNameUnique] = useState(true);

  useEffect(() => {
    if (!isReadOnly) {
      // Sync changes with parent component only if fields are editable
      onChange({ name: characterName, race, characterClass });
    }
  }, [characterName, race, characterClass]);

  const handleBlur = async () => {
    if (characterName && !isReadOnly && !characterCreated) {
      // Check name uniqueness only if not in read-only mode and character is not created
      try {
        const unique = await checkCharacterNameUnique(characterName);
        setIsNameUnique(unique);
      } catch (error) {
        console.error('Error checking name uniqueness:', error);
        setIsNameUnique(false); // Assume not unique if there's an error
      }
    }
  };

  return (
    <div className="basic-info-step">
      <h2>Basic Information</h2>
      <div className="form-group">
        <label htmlFor="characterName">Character Name:</label>
        <input
          id="characterName"
          type="text"
          value={characterName}
          onChange={(e) => setCharacterName(e.target.value)}
          onBlur={handleBlur}
          readOnly={isReadOnly} // Set input to read-only if isReadOnly is true
        />
        {!isNameUnique && !characterCreated && <p className="error-text">Name is already taken, please choose another one.</p>}
      </div>
      <div className="form-group">
        <label htmlFor="race">Race:</label>
        <select id="race" value={race} onChange={(e) => setRace(e.target.value)} disabled={isReadOnly}>
          <option value="">Select Race</option>
          <option value="Human">Human</option>
          <option value="Elf">Elf</option>
          {/* Add more options as needed */}
        </select>
      </div>
      <div className="form-group">
        <label htmlFor="characterClass">Character Class:</label>
        <select id="characterClass" value={characterClass} onChange={(e) => setCharacterClass(e.target.value)} disabled={isReadOnly}>
          <option value="">Select Class</option>
          <option value="Warrior">Warrior</option>
          <option value="Mage">Mage</option>
          {/* Add more options as needed */}
        </select>
      </div>
    </div>
  );
};

export default BasicInfoStep;
