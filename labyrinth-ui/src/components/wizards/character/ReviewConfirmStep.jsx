import React from 'react';
import { Button } from 'antd';
import './ReviewConfirmStep.css';

const ReviewConfirmStep = ({ formData }) => {
  return (
    <div className="review-confirm-step">
      <h2>Review Your Character</h2>
      <p><strong>Name:</strong> {formData.name}</p>
      <p><strong>Race:</strong> {formData.race}</p>
      <p><strong>Class:</strong> {formData.characterClass}</p>
      <h3>Stats:</h3>
      <ul>
        {Object.entries(formData.stats).map(([stat, value]) => (
          <li key={stat}><strong>{stat}:</strong> {value}</li>
        ))}
      </ul>
      <Button type="primary" onClick={() => console.log('Character Created!', formData)}>
        Confirm and Create Character
      </Button>
    </div>
  );
};

export default ReviewConfirmStep;
