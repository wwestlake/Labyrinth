import React from 'react';

const CharacterSheet = ({ character }) => {
  return (
    <div className="character-sheet">
      <h3>{character.name}</h3>
      <p>Level: {character.level}</p>
      <p>Class: {character.class}</p>
      <p>Health: {character.health}</p>
      {/* Add more character details as needed */}
    </div>
  );
};

export default CharacterSheet;
