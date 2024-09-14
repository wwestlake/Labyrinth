import { auth } from '../firebase';
import { createUserWithEmailAndPassword, signInWithEmailAndPassword, getIdToken, signOut, onIdTokenChanged } from 'firebase/auth';
import axios from 'axios';

const API_BASE_URL = 'https://localhost:5001/api'; // Base URL of your backend API

// Store token in local storage (or session storage or memory)
const storeToken = (token) => {
    localStorage.setItem('token', token);
};

// Get token from local storage
export const getStoredToken = () => {
    return localStorage.getItem('token');
};

// Store user credentials in local storage if "Remember Me" is checked
const storeUserCredentials = (email, password) => {
    localStorage.setItem('storedEmail', email);
    localStorage.setItem('storedPassword', password);
};

// Get stored user credentials
export const getStoredCredentials = () => {
    const email = localStorage.getItem('storedEmail');
    const password = localStorage.getItem('storedPassword');
    if (email && password) {
        return { email, password };
    }
    return null;
};

// Clear stored user credentials
const clearStoredCredentials = () => {
    localStorage.removeItem('storedEmail');
    localStorage.removeItem('storedPassword');
};

// Function to handle user signup
export const signup = async (email, password, displayName) => {
    try {
        const userCredential = await createUserWithEmailAndPassword(auth, email, password);
        const user = userCredential.user;

        const token = await getIdToken(user);
        storeToken(token); // Store the token after getting it

        const response = await axios.post(`${API_BASE_URL}/User`, {
            userId: user.uid,
            email: user.email,
            displayName: displayName,
            role: 'Player',
            description: '',
            settings: {
                receiveNotifications: true,
                preferredLanguage: 'en',
                darkMode: true
            }
        }, {
            headers: { Authorization: `Bearer ${token}` }
        });

        return response.data;
    } catch (error) {
        console.error("Error signing up:", error);
        throw error;
    }
};

// Function to handle user login
export const login = async (email, password, rememberMe) => {
    try {
        const userCredential = await signInWithEmailAndPassword(auth, email, password);
        const user = userCredential.user;

        const token = await getIdToken(user);
        storeToken(token); // Store the token after getting it

        // Store credentials if "Remember Me" is checked
        if (rememberMe) {
            storeUserCredentials(email, password);
        } else {
            clearStoredCredentials();
        }

        await setCustomClaims(user.uid, token);

        return { user, token };
    } catch (error) {
        console.error("Error logging in:", error);
        throw error;
    }
};

// Function to set custom claims in the backend
export const setCustomClaims = async (firebaseUid, token) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/auth/set-custom-claims`, 
            { FirebaseUid: firebaseUid }, 
            { headers: { Authorization: `Bearer ${token}` } }
        );

        if (response.status !== 200) {
            throw new Error('Failed to set custom claims.');
        }

        return response.data;
    } catch (error) {
        console.error("Error setting custom claims:", error);
        throw error;
    }
};

// Function to sign out the user
export const logout = async () => {
    try {
        await signOut(auth);
        localStorage.removeItem('token'); // Clear the token on logout
        clearStoredCredentials(); // Clear stored credentials on logout
    } catch (error) {
        console.error("Error signing out:", error);
        throw error;
    }
};

// Function to refresh the token if necessary (Firebase automatically refreshes tokens)
export const refreshToken = async () => {
    try {
        const user = auth.currentUser;
        if (user) {
            const token = await getIdToken(user, true); // Force refresh token
            storeToken(token); // Store the refreshed token
            return token;
        } else {
            throw new Error("No user is currently logged in.");
        }
    } catch (error) {
        console.error("Error refreshing token:", error);
        throw error;
    }
};

// Helper function to get a valid token
export const getValidToken = async () => {
    const user = auth.currentUser;
    if (!user) {
        throw new Error("No user is currently logged in.");
    }

    const token = await getIdToken(user);
    storeToken(token); // Ensure the latest token is stored
    return token;
};

// Listen for token changes and update the stored token
onIdTokenChanged(auth, async (user) => {
    if (user) {
        const token = await getIdToken(user);
        storeToken(token);
    } else {
        localStorage.removeItem('token'); // Remove token if user logs out or no user is logged in
    }
});
