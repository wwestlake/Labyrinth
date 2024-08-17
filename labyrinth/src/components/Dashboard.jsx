import React from 'react';
import { Container, Row, Col, Card } from 'react-bootstrap';

const Dashboard = () => {
  const cardData = [
    { title: 'User Profile' },
    { title: 'Characters' },
    { title: 'Game Stats' },
    { title: 'Top Players' },
    { title: 'Notices' },
    { title: 'Inbox' },
    { title: 'Server Status' },
    { title: 'Game Status' },
    { title: 'General Information' },
  ];

  return (
    <Container fluid className="mt-3">
      <Row className="g-1" style={{ marginLeft: '5px', marginRight: '5px' }}>
        {cardData.map((card, index) => (
          <Col key={index} xs={12} sm={6} md={4}>
            <Card className="h-100" style={{ borderRadius: '10px', borderWidth: '1px', borderColor: '#ccc' }}>
              <Card.Header className="bg-light" style={{ borderTopLeftRadius: '10px', borderTopRightRadius: '10px' }}>
                <h5>{card.title}</h5>
              </Card.Header>
              <Card.Body className="bg-dark text-white" style={{ borderBottomLeftRadius: '10px', borderBottomRightRadius: '10px' }}>
                <p>Content for {card.title}...</p>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </Container>
  );
};

export default Dashboard;
