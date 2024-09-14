// src/components/SignInForm.jsx
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { login, getStoredToken, getStoredCredentials } from '../services/authService';
import './SignInForm.css';

const SignInForm = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const token = getStoredToken();
    if (token) {
      navigate('/game'); // Automatically navigate to game if a valid token exists
    } else {
      const storedCredentials = getStoredCredentials();
      if (storedCredentials) {
        setEmail(storedCredentials.email);
        setPassword(storedCredentials.password);
        setRememberMe(true);
      }
    }
  }, [navigate]);

  const handleSignIn = async (event) => {
    event.preventDefault();
    setLoading(true);
    setErrorMessage('');

    try {
      const { user, token } = await login(email, password, rememberMe);
      console.log("User logged in:", user);
      navigate('/game');
    } catch (error) {
      setErrorMessage('Failed to sign in. Please check your credentials.');
      console.error("Sign-in error:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="signin-container">
      <form onSubmit={handleSignIn} className="needs-validation" noValidate>
        <div className="form-row">
          <div className="col-md-12 mb-3">
            <label htmlFor="validationEmail">Email</label>
            <input
              type="email"
              className="form-control"
              id="validationEmail"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
            <div className="invalid-feedback">
              Please enter a valid email address.
            </div>
          </div>
          <div className="col-md-12 mb-3">
            <label htmlFor="validationPassword">Password</label>
            <input
              type="password"
              className="form-control"
              id="validationPassword"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <div className="invalid-feedback">
              Please enter your password.
            </div>
          </div>
        </div>
        <div className="form-group">
          <div className="form-check">
            <input
              className="form-check-input"
              type="checkbox"
              checked={rememberMe}
              id="rememberMeCheck"
              onChange={() => setRememberMe(!rememberMe)}
            />
            <label className="form-check-label" htmlFor="rememberMeCheck">
              Remember me
            </label>
          </div>
        </div>
        <button className="btn btn-primary" type="submit" disabled={loading}>
          {loading ? 'Signing In...' : 'Sign In'}
        </button>
        {errorMessage && <div className="alert alert-danger mt-3">{errorMessage}</div>}
        <div className="mt-3">
          <a href="/forgot-password">Forgot Password?</a> |{' '}
          <a href="/signup">Create an Account</a>
        </div>
      </form>
    </div>
  );
};

export default SignInForm;
