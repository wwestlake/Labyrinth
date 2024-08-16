import React, { useState } from 'react';
import { signInWithEmailAndPassword } from 'firebase/auth';
import { auth } from '../firebase-config';
import './Signup.css'; // Reuse the same styles for consistency
import { useNavigate } from 'react-router-dom'; // Import useNavigate

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate(); // Initialize the useNavigate hook

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const userCredential = await signInWithEmailAndPassword(auth, email, password);

      // Fetch the user from the Labyrinth API
      const response = await fetch(`http://localhost:5232/api/User/${userCredential.user.uid}`, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${await userCredential.user.getIdToken()}`
        }
      });

      if (!response.ok) {
        throw new Error('Failed to fetch user from Labyrinth API');
      }

      const user = await response.json();
      console.log('User fetched from API:', user);

      setErrorMessage(""); // Clear any previous error message
      navigate('/dashboard'); // Redirect to the Dashboard page after successful login
    } catch (error) {
      setErrorMessage(error.message);
    }
  };

  return (
    <div className="signup-form">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button type="submit">Login</button>
      </form>
      {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
    </div>
  );
};

export default Login;
