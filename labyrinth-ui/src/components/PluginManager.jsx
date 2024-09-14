import React, { useState } from 'react';
import { Tab, Nav, Row, Col } from 'react-bootstrap';
import PluginList from './PluginList';
import PluginEditor from './PluginEditor';
import './PluginManager.css'; // Optional for any custom styling

const PluginManager = () => {
  const [key, setKey] = useState('list');

  return (
    <div className="plugin-manager-container">
      <Tab.Container id="plugin-tabs" activeKey={key} onSelect={(k) => setKey(k)}>
        <Row className="mb-3">
          <Col sm={12}>
            <Nav variant="tabs">
              <Nav.Item>
                <Nav.Link eventKey="list">Plugin List</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="editor">Plugin Editor</Nav.Link>
              </Nav.Item>
            </Nav>
          </Col>
        </Row>

        <Row>
          <Col sm={12}>
            <Tab.Content>
              <Tab.Pane eventKey="list">
                <PluginList />
              </Tab.Pane>
              <Tab.Pane eventKey="editor">
                <PluginEditor />
              </Tab.Pane>
            </Tab.Content>
          </Col>
        </Row>
      </Tab.Container>
    </div>
  );
};

export default PluginManager;
