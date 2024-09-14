// src/services/pluginService.js
import axios from 'axios';
import { getValidToken } from './authService'; // Reuse token retrieval

const API_BASE_URL = 'https://localhost:5001/api/plugin'; // Change the base URL to match your plugins API endpoint

// Fetch the list of plugins
export const getPlugins = async () => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send GET request to fetch plugins list
    const response = await axios.get(`${API_BASE_URL}`, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
    return response.data; // Return the list of plugins
  } catch (error) {
    console.error('Error fetching plugins:', error);
    throw error;
  }
};

// Enable or Disable a plugin
export const togglePluginStatus = async (pluginId, status) => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send PUT request to update plugin status (enabled/disabled)
    const response = await axios.put(`${API_BASE_URL}/${pluginId}/status`, { status }, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
    return response.data; // Return updated plugin data
  } catch (error) {
    console.error(`Error ${status === 'enabled' ? 'enabling' : 'disabling'} plugin:`, error);
    throw error;
  }
};

// Delete a plugin
export const deletePlugin = async (pluginId) => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send DELETE request to remove plugin
    await axios.delete(`${API_BASE_URL}/${pluginId}`, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
  } catch (error) {
    console.error('Error deleting plugin:', error);
    throw error;
  }
};

// Fetch plugin metadata
export const getPluginMetadata = async (pluginId) => {
  const token = await getValidToken(); // Get the valid token
  
  try {
    // Send GET request to retrieve plugin metadata
    const response = await axios.get(`${API_BASE_URL}/${pluginId}/metadata`, {
      headers: { Authorization: `Bearer ${token}` }, // Include the token in the request
    });
    return response.data; // Return plugin metadata
  } catch (error) {
    console.error('Error fetching plugin metadata:', error);
    throw error;
  }
};
