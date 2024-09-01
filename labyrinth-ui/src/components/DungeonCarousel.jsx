// src/components/DungeonCarousel.jsx
import React from 'react';
import './DungeonCarousel.css'; // Import the CSS file

const DungeonCarousel = () => {
  return (
    <div id="dungeonCarousel" className="carousel slide" data-bs-ride="carousel">
      {/* Text Overlay */}
      <div className="carousel-overlay">
        <h1 className="carousel-title">Labyrinth</h1>
      </div>

      <div className="carousel-indicators">
        <button
          type="button"
          data-bs-target="#dungeonCarousel"
          data-bs-slide-to="0"
          className="active"
          aria-current="true"
          aria-label="Slide 1"
        ></button>
        <button
          type="button"
          data-bs-target="#dungeonCarousel"
          data-bs-slide-to="1"
          aria-label="Slide 2"
        ></button>
        <button
          type="button"
          data-bs-target="#dungeonCarousel"
          data-bs-slide-to="2"
          aria-label="Slide 3"
        ></button>
        <button
          type="button"
          data-bs-target="#dungeonCarousel"
          data-bs-slide-to="3"
          aria-label="Slide 4"
        ></button>
      </div>
      <div className="carousel-inner">
        <div className="carousel-item active">
          <img
            src="/images/DungeonCell.webp"
            className="d-block w-100 carousel-image"
            alt="Dungeon Cell"
          />
        </div>
        <div className="carousel-item">
          <img
            src="/images/DungeonCorridor.webp"
            className="d-block w-100 carousel-image"
            alt="Dungeon Corridor"
          />
        </div>
        <div className="carousel-item">
          <img
            src="/images/MainDungeonEntrance.webp"
            className="d-block w-100 carousel-image"
            alt="Main Dungeon Entrance"
          />
        </div>
        <div className="carousel-item">
          <img
            src="/images/TortureChamber.webp"
            className="d-block w-100 carousel-image"
            alt="Torture Chamber"
          />
        </div>
      </div>
      <button
        className="carousel-control-prev"
        type="button"
        data-bs-target="#dungeonCarousel"
        data-bs-slide="prev"
      >
        <span className="carousel-control-prev-icon" aria-hidden="true"></span>
        <span className="visually-hidden">Previous</span>
      </button>
      <button
        className="carousel-control-next"
        type="button"
        data-bs-target="#dungeonCarousel"
        data-bs-slide="next"
      >
        <span className="carousel-control-next-icon" aria-hidden="true"></span>
        <span className="visually-hidden">Next</span>
      </button>
    </div>
  );
};

export default DungeonCarousel;
