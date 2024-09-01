// src/services/authService.js
import { auth } from '../firebase';
import { createUserWithEmailAndPassword, signInWithEmailAndPassword, getIdToken, signOut } from 'firebase/auth';
import axios from 'axios';

const API_BASE_URL = 'http://localhost:5232/api'; // Base URL of your backend API

// Function to handle user signup
export const signup = async (email, password, displayName) => {
    try {
        // Step 1: Create a user with Firebase Authentication
        const userCredential = await createUserWithEmailAndPassword(auth, email, password);
        const user = userCredential.user;

        // Step 2: Get the Firebase token
        const token = await getIdToken(user);

        // Step 3: Create user in the backend
        const response = await axios.post(`${API_BASE_URL}/User`, {
            userId: user.uid,
            email: user.email,
            displayName: displayName,
            role: 'Player', // Default role for new users
            description: '',
            settings: {
                receiveNotifications: true,
                preferredLanguage: 'en',
                darkMode: true
            }
        }, {
            headers: { Authorization: `Bearer ${token}` } // Pass the Firebase token to the backend
        });

        return response.data; // Return the created user data
    } catch (error) {
        console.error("Error signing up:", error);
        throw error;
    }
};

// Function to handle user login
export const login = async (email, password) => {
    try {
        // Step 1: Sign in with Firebase Authentication
        const userCredential = await signInWithEmailAndPassword(auth, email, password);
        const user = userCredential.user;

        // Step 2: Get the Firebase token
        const token = await getIdToken(user);

        // Step 3: Set custom claims in the backend
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

        return response.data; // Return response data if needed
    } catch (error) {
        console.error("Error setting custom claims:", error);
        throw error;
    }
};

// Function to sign out the user
export const logout = async () => {
    try {
        await signOut(auth);
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
            return token;
        } else {
            throw new Error("No user is currently logged in.");
        }
    } catch (error) {
        console.error("Error refreshing token:", error);
        throw error;
    }
};
