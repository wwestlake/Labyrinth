// src/context/GameContext.js
import React, { createContext, useContext, useState } from 'react';

const GameContext = createContext();

export const useGameContext = () => useContext(GameContext);

export const GameProvider = ({ children }) => {
  const [roomState, setRoomState] = useState({}); // Example state for Room
  const [characterState, setCharacterState] = useState({}); // Example state for Character

  return (
    <GameContext.Provider value={{ roomState, setRoomState, characterState, setCharacterState }}>
      {children}
    </GameContext.Provider>
  );
};
