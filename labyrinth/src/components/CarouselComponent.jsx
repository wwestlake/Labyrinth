import React, { useState } from 'react';
import './CarouselComponent.css'; // Add custom styles as needed

const CarouselComponent = ({ items }) => {
  const [currentIndex, setCurrentIndex] = useState(0);

  const goToNext = () => {
    setCurrentIndex((prevIndex) => (prevIndex + 1) % items.length);
  };

  const goToPrev = () => {
    setCurrentIndex((prevIndex) => (prevIndex - 1 + items.length) % items.length);
  };

  return (
    <div className="carousel-container">
      <div className="carousel-item">
        {items[currentIndex]}
      </div>
      <button onClick={goToPrev} className="carousel-control prev">‹</button>
      <button onClick={goToNext} className="carousel-control next">›</button>
    </div>
  );
};

export default CarouselComponent;
