import React, { useState, useEffect, useRef } from 'react';
import './ChatClient.css'; // Ensure you have the CSS styles

const ChatClient = () => {
  const [participants, setParticipants] = useState([]);
  const [messages, setMessages] = useState([]);
  const [message, setMessage] = useState('');
  const [sendOnEnter, setSendOnEnter] = useState(
    JSON.parse(localStorage.getItem('sendOnEnter')) ?? true
  );
  const [darkMode, setDarkMode] = useState(
    JSON.parse(localStorage.getItem('darkMode')) ?? false
  );

  const messageEndRef = useRef(null);

  useEffect(() => {
    // Example: Simulate receiving a new message
    const newMessage = {
      user: 'System',
      text: 'Welcome to the chat!',
      timestamp: new Date(),
    };
    setMessages((prevMessages) => [...prevMessages, newMessage]);

    // Scroll to the bottom when a new message is added
    messageEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, []);

  useEffect(() => {
    // Save settings to local storage whenever they change
    localStorage.setItem('sendOnEnter', JSON.stringify(sendOnEnter));
    localStorage.setItem('darkMode', JSON.stringify(darkMode));
  }, [sendOnEnter, darkMode]);

  const handleSendMessage = () => {
    if (!message.trim()) return;

    const newMessage = {
      user: 'You',
      text: message,
      timestamp: new Date(),
    };

    setMessages((prevMessages) => [...prevMessages, newMessage]);
    setMessage('');
  };

  const handleKeyPress = (e) => {
    if (sendOnEnter && e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSendMessage();
    } else if (!sendOnEnter && e.key === 'Enter' && e.ctrlKey) {
      e.preventDefault();
      handleSendMessage();
    }
  };

  const toggleSendMode = () => setSendOnEnter(!sendOnEnter);
  const toggleDarkMode = () => setDarkMode(!darkMode);

  return (
    <div className={`chat-client ${darkMode ? 'dark-mode' : 'light-mode'}`}>
      <div className="participants-list">
        <h4>Participants</h4>
        {participants.length > 0 ? (
          participants.map((participant, index) => (
            <div key={index} className="participant">
              {participant}
            </div>
          ))
        ) : (
          <p>No participants yet</p>
        )}
      </div>
      <div className="messages-container">
        <div className="messages">
          {messages.map((msg, index) => (
            <div key={index} className="message">
              <span className="message-user">{msg.user}: </span>
              <span className="message-text">{msg.text}</span>
            </div>
          ))}
          <div ref={messageEndRef} />
        </div>
        <div className="message-input-container">
          <textarea
            className="message-input"
            placeholder="Type your message..."
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            onKeyPress={handleKeyPress}
            rows="1"
          />
          <button className="send-button" onClick={handleSendMessage}>
            Send
          </button>
        </div>
        <div className="settings">
          <button onClick={toggleSendMode}>
            Send on {sendOnEnter ? 'Enter' : 'Ctrl+Enter'}
          </button>
          <button onClick={toggleDarkMode}>
            Switch to {darkMode ? 'Light Mode' : 'Dark Mode'}
          </button>
        </div>
      </div>
    </div>
  );
};

export default ChatClient;
