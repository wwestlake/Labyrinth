// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth"; // Import getAuth for authentication
import { getAnalytics } from "firebase/analytics";

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyCt8Hm_zoz83IjjDAPldaJZtkB1UJXthrk",
  authDomain: "lagdaemon-game-authentication.firebaseapp.com",
  projectId: "lagdaemon-game-authentication",
  storageBucket: "lagdaemon-game-authentication.appspot.com",
  messagingSenderId: "765911219149",
  appId: "1:765911219149:web:33ac437944466d2db4e284",
  measurementId: "G-JH1KX5EEG4"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
export const auth = getAuth(app); // Export auth for authentication

export default app;
