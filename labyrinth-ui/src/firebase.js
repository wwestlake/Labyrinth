// src/firebase.js
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";
import { getFirestore } from "firebase/firestore";

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

// Initialize Firebase services
const auth = getAuth(app);
const db = getFirestore(app);

// Export Firebase services
export { auth, db };
