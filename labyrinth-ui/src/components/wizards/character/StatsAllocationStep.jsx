import React, { useState, useEffect } from 'react';
import { InputNumber, Row, Col } from 'antd';
import './StatsAllocationStep.css';

const StatsAllocationStep = ({ onChange, formData }) => {
  const initialStats = formData?.stats || {
    Health: 10,
    Mana: 10,
    Strength: 10,
    Dexterity: 10,
    Constitution: 10,
    Intelligence: 10,
    Wisdom: 10,
    Charisma: 10,
    Luck: 10,
  };

  const [stats, setStats] = useState(initialStats);
  const [bonusPoints, setBonusPoints] = useState(formData?.bonusPoints || 10);

  useEffect(() => {
    setStats(formData?.stats || initialStats);
    setBonusPoints(formData?.bonusPoints || 10);
  }, [formData]);

  const handleStatChange = (stat, value) => {
    const pointDifference = value - stats[stat];
    if (bonusPoints - pointDifference >= 0) {
      const updatedStats = { ...stats, [stat]: value };
      setStats(updatedStats);
      setBonusPoints(bonusPoints - pointDifference);
      onChange({ stats: updatedStats, bonusPoints: bonusPoints - pointDifference });
    }
  };

  return (
    <div className="stats-allocation-step">
      <h2>Allocate Stats</h2>
      <p>Bonus Points Remaining: {bonusPoints}</p>
      <Row gutter={16}>
        {Object.keys(stats).map((stat) => (
          <Col span={8} key={stat}>
            <label>{stat}</label>
            <InputNumber
              min={0}
              value={stats[stat]}
              onChange={(value) => handleStatChange(stat, value)}
            />
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default StatsAllocationStep;
