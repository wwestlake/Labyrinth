import React, { useState, useEffect } from 'react';
import { createUserWithEmailAndPassword, sendEmailVerification, onAuthStateChanged } from 'firebase/auth';
import { auth } from '../firebase-config';
import './Signup.css';
import { useNavigate } from 'react-router-dom';

const Signup = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [passwordStrength, setPasswordStrength] = useState(0);
  const [passwordMatch, setPasswordMatch] = useState(true);
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const minPasswordStrength = 3; // Configurable minimum allowed strength

  const handleSignup = async (e) => {
    e.preventDefault();

    if (password !== confirmPassword) {
      setErrorMessage("Passwords do not match.");
      return;
    }

    if (passwordStrength < minPasswordStrength) {
      setErrorMessage(`Password is too weak. Minimum strength required is ${minPasswordStrength}.`);
      return;
    }

    try {
      const userCredential = await createUserWithEmailAndPassword(auth, email, password);
      await sendEmailVerification(userCredential.user);

      console.log("Sending request to API...");

      const response = await fetch('http://localhost:5232/api/User', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${await userCredential.user.getIdToken()}`
        },
        body: JSON.stringify({
          userId: userCredential.user.uid,  // Required
          email: userCredential.user.email, // Required
          displayName: userCredential.user.email, // Example display name, can be customized
          role: 'User', // Required, make sure it matches the enum on the server
          description: "",
          settings: {
            receiveNotifications: false,
            preferredLanguage: "none",
            darkMode: false
          }
        })
      });
      
      console.log("Response status:", response.status);

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to create user in Labyrinth API: ${response.status} - ${errorText}`);
      }

      alert('Account created! Please check your email to verify your account.');
    } catch (error) {
      console.error("Error during signup:", error);
      setErrorMessage(error.message);
    }
  };

  useEffect(() => {
    const unsubscribe = onAuthStateChanged(auth, (user) => {
      if (user) {
        if (user.emailVerified) {
          navigate('/dashboard');
        } else {
          alert('Please verify your email to continue.');
        }
      }
    });

    return () => unsubscribe();
  }, [navigate]);

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  const evaluatePasswordStrength = (password) => {
    let strength = 0;
    if (password.length >= 8) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[@$!%*?&#]/.test(password)) strength++;
    return strength;
  };

  useEffect(() => {
    setPasswordStrength(evaluatePasswordStrength(password));
    setPasswordMatch(password === confirmPassword);
  }, [password, confirmPassword]);

  return (
    <div className="signup-form">
      <h2>Sign Up</h2>
      <form onSubmit={handleSignup}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <div className="password-field">
          <input
            type={passwordVisible ? "text" : "password"}
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button type="button" onClick={togglePasswordVisibility}>
            {passwordVisible ? "Hide" : "Show"}
          </button>
        </div>
        <div className="password-strength">
          Password Strength: {passwordStrength} / 4
        </div>
        <input
          type={passwordVisible ? "text" : "password"}
          placeholder="Confirm Password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />
        {!passwordMatch && <p style={{ color: 'red' }}>Passwords do not match.</p>}
        <button type="submit">Sign Up</button>
      </form>
      {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
    </div>
  );
};

export default Signup;
