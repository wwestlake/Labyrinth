import React, { useState } from 'react';
import { Steps, Button, message } from 'antd';
import BasicInfoStep from './BasicInfoStep';
import StatsAllocationStep from './StatsAllocationStep';
import ReviewConfirmStep from './ReviewConfirmStep';
import { createCharacter, updateCharacterStats } from '../../../services/characterService';
import './CharacterCreationWizard.css';

const { Step } = Steps;

const CharacterCreationWizard = () => {
  const [currentStep, setCurrentStep] = useState(0);
  const [formData, setFormData] = useState({
    name: '',
    race: '',
    characterClass: '',
    stats: {
      Health: 10,
      Mana: 10,
      Strength: 10,
      Dexterity: 10,
      Constitution: 10,
      Intelligence: 10,
      Wisdom: 10,
      Charisma: 10,
      Luck: 10,
    },
    bonusPoints: 10,
  });

  const [character, setCharacter] = useState(null); // Store the character object returned from the server
  const [loading, setLoading] = useState(false); // Loading state for API calls

  const handleNext = async () => {
    if (currentStep === 0 && !character) {
      // If on the first step and character has not been created yet
      try {
        setLoading(true);
        const createdCharacter = await createCharacter(formData); // Call the API to create the character
        
        if (createdCharacter) { // Ensure character is created successfully
          setCharacter(createdCharacter); // Set the created character from the server
          setFormData({ ...formData, ...createdCharacter }); // Sync form data with server response
          setCurrentStep((prevStep) => prevStep + 1); // Advance to the next step
        } else {
          message.error('Failed to create character. Please try again.');
        }
      } catch (error) {
        message.error('Failed to create character. Please try again.');
        console.error('Error creating character:', error); // Log error for debugging
      } finally {
        setLoading(false);
      }
    } else {
      // For other steps, simply advance
      setCurrentStep((prevStep) => prevStep + 1);
    }
  };

  const handlePrev = () => {
    if (currentStep === 1) {
      // Prevent going back to the basic info step if the character has been created
      return;
    }
    setCurrentStep((prevStep) => prevStep - 1);
  };

  const handleChange = (newData) => {
    setFormData((prevData) => ({ ...prevData, ...newData }));
  };

  const handleUpdateStats = async (newStats) => {
    if (character) {
      try {
        setLoading(true);
        const updatedCharacter = await updateCharacterStats(character.id, newStats);
        setCharacter(updatedCharacter); // Update the character state with the updated stats from the server
        setFormData({ ...formData, stats: updatedCharacter.stats, bonusPoints: updatedCharacter.bonusPoints });
      } catch (error) {
        message.error('Failed to update character stats. Please try again.');
      } finally {
        setLoading(false);
      }
    }
  };

  return (
    <div className="character-creation-wizard">
      <Steps current={currentStep}>
        <Step title="Basic Info" />
        <Step title="Allocate Stats" />
        <Step title="Review & Confirm" />
      </Steps>

      <div className="steps-content">
        {currentStep === 0 && (
          <BasicInfoStep 
            onChange={handleChange} 
            formData={formData} 
            isReadOnly={character !== null} // Make fields read-only if the character is created
          />
        )}
        {currentStep === 1 && (
          <StatsAllocationStep onChange={handleUpdateStats} formData={formData} />
        )}
        {currentStep === 2 && (
          <ReviewConfirmStep formData={formData} />
        )}
      </div>

      <div className="steps-action">
        {currentStep > 0 && (
          <Button style={{ margin: '0 8px' }} onClick={handlePrev}>
            Previous
          </Button>
        )}
        {currentStep < 2 && (
          <Button type="primary" onClick={handleNext} loading={loading}>
            Next
          </Button>
        )}
        {currentStep === 2 && (
          <Button
            type="primary"
            onClick={() => console.log('Character Created!', formData)}
            loading={loading}
          >
            Finish
          </Button>
        )}
      </div>
    </div>
  );
};

export default CharacterCreationWizard;
