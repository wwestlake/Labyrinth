// src/services/characterService.js
import axios from 'axios';
import { getValidToken } from './authService'; // Ensure you have this function to get the token

const API_BASE_URL = 'http://localhost:5232/api/Character'; // Base URL of your backend API

// Function to create a new character
export const createCharacter = async (characterData) => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send a POST request to create a character
    const response = await axios.post(`${API_BASE_URL}/Generate`, characterData, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
    return response.data; // Return the created character data from the response
  } catch (error) {
    console.error('Error creating character:', error); // Log error for debugging
    throw error; // Re-throw error to be handled by calling function
  }
};

// Function to update character stats
export const updateCharacterStats = async (characterId, statsData) => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send a PUT request to update character stats
    const response = await axios.put(`${API_BASE_URL}/Update/${characterId}`, statsData, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
    return response.data; // Return updated character data from the response
  } catch (error) {
    console.error('Error updating character stats:', error); // Log error for debugging
    throw error; // Re-throw error to be handled by calling function
  }
};

// Function to check if a character name is unique
export const checkCharacterNameUnique = async (name) => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send a GET request to check if the character name is unique
    const response = await axios.get(`${API_BASE_URL}/check-name-unique?name=${name}`, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
    return response.data.isUnique; // Return whether the name is unique
  } catch (error) {
    console.error('Error checking name uniqueness:', error); // Log error for debugging
    throw error; // Re-throw error to be handled by calling function
  }
};
