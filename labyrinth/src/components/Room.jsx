import React from 'react';
import { Container, Row, Col, Card, ListGroup, Button } from 'react-bootstrap';

const Room = () => {
  return (
    <Container fluid className="mt-3">
      <Row>
        {/* Top Left: Room Information */}
        <Col xs={12} md={6} lg={6}>
          <Card className="mb-3">
            <Card.Header className="bg-light">
              <h5>Room Name</h5>
            </Card.Header>
            <Card.Body>
              <p>This is a placeholder for the room description. It will include details about the room, visible objects, and other pertinent information.</p>
              <ul>
                <li>Visible Object 1</li>
                <li>Visible Object 2</li>
                <li>Visible Object 3</li>
              </ul>
            </Card.Body>
          </Card>
        </Col>

        {/* Top Right: Room Graphic */}
        <Col xs={12} md={6} lg={6}>
          <Card className="mb-3">
            <Card.Header className="bg-light">
              <h5>Room Graphic</h5>
            </Card.Header>
            <Card.Body className="d-flex justify-content-center align-items-center">
              <img src="https://via.placeholder.com/150" alt="Room Graphic" className="img-fluid" />
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <Row>
        {/* Right Side: Player List */}
        <Col xs={12} md={4} lg={4}>
          <Card className="mb-3">
            <Card.Header className="bg-light">
              <h5>Players in Room</h5>
            </Card.Header>
            <ListGroup variant="flush">
              <ListGroup.Item>Player 1 - Character A</ListGroup.Item>
              <ListGroup.Item>Player 2 - Character B</ListGroup.Item>
              <ListGroup.Item>Player 3 - Character C</ListGroup.Item>
            </ListGroup>
          </Card>
        </Col>

        {/* Bottom Left: Chat Area */}
        <Col xs={12} md={8} lg={8}>
          <Card className="mb-3">
            <Card.Header className="bg-light">
              <h5>Room Chat</h5>
            </Card.Header>
            <Card.Body style={{ height: '200px', overflowY: 'auto', backgroundColor: '#333', color: '#fff' }}>
              <p>[Player 1]: Hello!</p>
              <p>[Player 2]: Welcome to the room.</p>
              <p>[System]: The game will start soon...</p>
              {/* Add more chat messages here */}
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <Row>
        {/* Bottom Right: Control Panel */}
        <Col xs={12}>
          <Card className="mb-3">
            <Card.Header className="bg-light">
              <h5>Control Panel</h5>
            </Card.Header>
            <Card.Body className="d-flex justify-content-around">
              <Button variant="primary">Action 1</Button>
              <Button variant="secondary">Action 2</Button>
              <Button variant="danger">Action 3</Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default Room;
