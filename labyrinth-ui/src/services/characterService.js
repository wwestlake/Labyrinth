import axios from 'axios';

const API_URL = 'http://localhost:5232/api/Character';

export const createCharacter = async (characterData) => {
  const response = await axios.post(`${API_URL}/Generate`, characterData);
  return response.data;
};

export const updateCharacterStats = async (characterId, statsData) => {
  const response = await axios.put(`${API_URL}/Update/${characterId}`, statsData);
  return response.data;
};
