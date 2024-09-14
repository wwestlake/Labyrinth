import React, { useState } from 'react';
import { signup } from '../services/authService';
import axios from 'axios';
import "./SignupForm.css";

const SignupForm = () => {
  // State management
  const [email, setEmail] = useState('');
  const [displayName, setDisplayName] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [termsAccepted, setTermsAccepted] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const [passwordStrength, setPasswordStrength] = useState(0); // Track password strength

  const checkDisplayNameAvailability = async () => {
    if (!displayName) return;
    try {
      const response = await axios.get(`http://localhost:5232/api/User/check-displayname?displayName=${displayName}`);
      if (!response.data.available) {
        setErrorMessage('Display name is already in use.');
      } else {
        setErrorMessage('');
      }
    } catch (error) {
      setErrorMessage('Error checking display name availability.');
    }
  };

  const evaluatePasswordStrength = (password) => {
    let strength = 0;
    if (password.length >= 8) strength += 1; // Minimum length
    if (/[A-Z]/.test(password)) strength += 1; // Contains uppercase letter
    if (/[a-z]/.test(password)) strength += 1; // Contains lowercase letter
    if (/[0-9]/.test(password)) strength += 1; // Contains number
    if (/[^A-Za-z0-9]/.test(password)) strength += 1; // Contains special character
    return strength;
  };

  const handlePasswordChange = (e) => {
    const newPassword = e.target.value;
    setPassword(newPassword);
    const strength = evaluatePasswordStrength(newPassword);
    setPasswordStrength(strength); // Update password strength
  };

  const handleSignup = async (event) => {
    event.preventDefault();
    setErrorMessage('');
    setSuccessMessage('');

    if (!termsAccepted) {
      setErrorMessage('You must accept the terms and conditions.');
      return;
    }

    if (password !== confirmPassword) {
      setErrorMessage('Passwords do not match.');
      return;
    }

    setLoading(true);
    try {
      await signup(email, password, displayName);
      setSuccessMessage('Account created successfully!');
    } catch (error) {
      setErrorMessage(error.message || 'Failed to create an account.');
    } finally {
      setLoading(false);
    }
  };

  const handleEmailChange = (e) => {
    const emailValue = e.target.value;
    setEmail(emailValue);
    if (emailValue) {
      setDisplayName(emailValue.split('@')[0]);
    }
  };

  const getPasswordStrengthLabel = (strength) => {
    switch (strength) {
      case 0:
      case 1:
        return 'Weak';
      case 2:
        return 'Moderate';
      case 3:
        return 'Strong';
      case 4:
      case 5:
        return 'Very Strong';
      default:
        return '';
    }
  };

  const getPasswordStrengthColor = (strength) => {
    switch (strength) {
      case 0:
      case 1:
        return 'red';
      case 2:
        return 'orange';
      case 3:
        return 'yellow';
      case 4:
      case 5:
        return 'green';
      default:
        return 'gray';
    }
  };

  return (
    <form onSubmit={handleSignup} className="signup-container needs-validation" noValidate>
      <div className="form-group">
        <label htmlFor="email">Email</label>
        <input
          type="email"
          className="form-control"
          id="email"
          value={email}
          onChange={handleEmailChange}
          required
        />
        <div className="valid-feedback">Looks good!</div>
        <div className="invalid-feedback">Please provide a valid email.</div>
      </div>

      <div className="form-group">
        <label htmlFor="displayName">Display Name</label>
        <input
          type="text"
          className={`form-control ${errorMessage ? 'is-invalid' : ''}`}
          id="displayName"
          value={displayName}
          onChange={(e) => setDisplayName(e.target.value)}
          onBlur={checkDisplayNameAvailability}
          required
        />
        {errorMessage && <div className="invalid-feedback">{errorMessage}</div>}
      </div>

      <div className="form-group">
        <label htmlFor="password">Password</label>
        <input
          type="password"
          className="form-control"
          id="password"
          value={password}
          onChange={handlePasswordChange} // Use handlePasswordChange instead of setPassword directly
          required
        />
        <div className="password-strength-bar" style={{ backgroundColor: getPasswordStrengthColor(passwordStrength), width: `${passwordStrength * 20}%`, height: '10px', marginTop: '5px' }} />
        <div className="password-strength-label">{getPasswordStrengthLabel(passwordStrength)}</div>
      </div>

      <div className="form-group">
        <label htmlFor="confirmPassword">Confirm Password</label>
        <input
          type="password"
          className="form-control"
          id="confirmPassword"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />
        <div className="invalid-feedback">Passwords do not match.</div>
      </div>

      <div className="form-group">
        <div className="form-check">
          <input
            className="form-check-input"
            type="checkbox"
            id="termsCheckbox"
            checked={termsAccepted}
            onChange={(e) => setTermsAccepted(e.target.checked)}
            required
          />
          <label className="form-check-label" htmlFor="termsCheckbox">
            I have read and agree to the <a href="/terms">terms and conditions</a> and <a href="/privacy">privacy policy</a>.
          </label>
          <div className="invalid-feedback">You must agree to the terms and conditions.</div>
        </div>
      </div>

      <button className="btn btn-primary" type="submit" disabled={loading}>
        {loading ? 'Creating Account...' : 'Create Account'}
      </button>

      {successMessage && <div className="alert alert-success mt-3">{successMessage}</div>}
      {errorMessage && <div className="alert alert-danger mt-3">{errorMessage}</div>}
    </form>
  );
};

export default SignupForm;
