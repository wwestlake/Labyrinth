import React, { useState } from 'react';
import { Button, Form, ListGroup, ButtonGroup } from 'react-bootstrap';
import Editor from '@monaco-editor/react'; // Monaco Editor for code editing
import './PluginEditor.css'; // Create this file for your custom styles

// Dummy data to simulate CodeStore objects
const dummyPlugins = [
  {
    id: 1,
    name: 'Plugin A',
    description: 'This is the first plugin',
    version: '1.0.0',
    language: 'CSharp',
    files: [{ name: 'Main.cs', code: '// C# Code here' }],
    lastModified: '2024-09-11'
  },
  {
    id: 2,
    name: 'Plugin B',
    description: 'This is the second plugin',
    version: '1.2.0',
    language: 'FSharp',
    files: [{ name: 'Main.fs', code: '// F# Code here' }],
    lastModified: '2024-09-10'
  }
];

const PluginEditor = () => {
  const [plugins, setPlugins] = useState(dummyPlugins); // List of all plugins
  const [selectedPlugin, setSelectedPlugin] = useState(null); // Currently selected plugin
  const [selectedFile, setSelectedFile] = useState(null); // Currently selected file for editing
  const [code, setCode] = useState(''); // Code for the selected file
  const [metadata, setMetadata] = useState({}); // Metadata for the selected plugin
  const [theme, setTheme] = useState('vs-dark'); // Editor theme
  const [fontSize, setFontSize] = useState(14); // Font size for editor

  // Function to handle plugin selection
  const handlePluginSelect = (plugin) => {
    setSelectedPlugin(plugin);
    setMetadata({
      name: plugin.name,
      description: plugin.description,
      version: plugin.version,
      language: plugin.language
    });
    setSelectedFile(null); // Clear file selection when a new plugin is selected
  };

  // Function to handle file selection
  const handleFileSelect = (file) => {
    setSelectedFile(file);
    setCode(file.code);
  };

  // Function to handle code changes in the editor
  const handleEditorChange = (newCode) => {
    setCode(newCode);
  };

  // Function to handle new plugin creation
  const handleNewPlugin = () => {
    const newPlugin = {
      id: plugins.length + 1,
      name: `New Plugin ${plugins.length + 1}`,
      description: '',
      version: '1.0.0',
      language: 'CSharp',
      files: [{ name: 'Main.cs', code: '' }],
      lastModified: new Date().toISOString().split('T')[0]
    };
    setPlugins([...plugins, newPlugin]);
    handlePluginSelect(newPlugin);
  };

  // Function to increase/decrease font size
  const increaseFontSize = () => setFontSize((size) => size + 1);
  const decreaseFontSize = () => setFontSize((size) => size - 1);

  // Function to handle theme change
  const handleThemeChange = (event) => {
    setTheme(event.target.value);
  };

  return (
    <div className="plugin-editor-container">
      {/* Sidebar with list of plugins */}
      <div className="plugin-list">
        <h4>Plugins</h4>
        <ListGroup>
          {plugins.map((plugin) => (
            <ListGroup.Item
              key={plugin.id}
              active={selectedPlugin && selectedPlugin.id === plugin.id}
              onClick={() => handlePluginSelect(plugin)}
            >
              {plugin.name}
            </ListGroup.Item>
          ))}
        </ListGroup>
        <Button variant="primary" onClick={handleNewPlugin} className="mt-3">
          New Plugin
        </Button>
      </div>

      {/* Plugin Metadata and File List */}
      <div className="plugin-details">
        {selectedPlugin && (
          <>
            <h4>Metadata</h4>
            <Form>
              <Form.Group controlId="pluginName">
                <Form.Label>Name</Form.Label>
                <Form.Control
                  type="text"
                  value={metadata.name}
                  onChange={(e) => setMetadata({ ...metadata, name: e.target.value })}
                />
              </Form.Group>

              <Form.Group controlId="pluginDescription">
                <Form.Label>Description</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={2}
                  value={metadata.description}
                  onChange={(e) => setMetadata({ ...metadata, description: e.target.value })}
                />
              </Form.Group>

              <Form.Group controlId="pluginVersion">
                <Form.Label>Version</Form.Label>
                <Form.Control
                  type="text"
                  value={metadata.version}
                  onChange={(e) => setMetadata({ ...metadata, version: e.target.value })}
                />
              </Form.Group>

              <Form.Group controlId="pluginLanguage">
                <Form.Label>Language</Form.Label>
                <Form.Control
                  type="text"
                  value={metadata.language}
                  onChange={(e) => setMetadata({ ...metadata, language: e.target.value })}
                />
              </Form.Group>
            </Form>

            <h4 className="mt-4">Files</h4>
            <ListGroup>
              {selectedPlugin.files.map((file, index) => (
                <ListGroup.Item
                  key={index}
                  active={selectedFile && selectedFile.name === file.name}
                  onClick={() => handleFileSelect(file)}
                >
                  {file.name}
                </ListGroup.Item>
              ))}
            </ListGroup>
          </>
        )}
      </div>

      {/* Code Editor Section */}
      <div className="code-editor">
        {selectedFile ? (
          <>
            <h4>Editing: {selectedFile.name}</h4>
            {/* Editor Settings Controls */}
            <div className="editor-controls">
              <Form.Group controlId="themeSelect" className="mb-2">
                <Form.Label>Theme</Form.Label>
                <Form.Control as="select" value={theme} onChange={handleThemeChange}>
                  <option value="vs-dark">Dark</option>
                  <option value="vs-light">Light</option>
                  <option value="hc-black">High Contrast</option>
                </Form.Control>
              </Form.Group>

              <ButtonGroup className="mb-2">
                <Button onClick={decreaseFontSize}>-</Button>
                <Button disabled>{fontSize}</Button>
                <Button onClick={increaseFontSize}>+</Button>
              </ButtonGroup>

              <Button variant="success" className="ml-2" onClick={() => alert('Save clicked')}>
                Save
              </Button>
              <Button variant="warning" className="ml-2">Compile</Button>
            </div>

            <Editor
              height="500px"
              theme={theme}
              language={selectedPlugin.language.toLowerCase()}
              value={code}
              onChange={handleEditorChange}
              options={{ fontSize: fontSize }}
            />
          </>
        ) : (
          <p>Select a file to start editing.</p>
        )}
      </div>
    </div>
  );
};

export default PluginEditor;
