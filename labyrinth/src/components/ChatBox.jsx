import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { auth } from '../firebase-config'; // Adjust the import path as needed
import './ChatBox.css';

const ChatBox = () => {
  const [connection, setConnection] = useState(null);
  const [currentChannel, setCurrentChannel] = useState('general');
  const [messages, setMessages] = useState([]);
  const [message, setMessage] = useState('');

  useEffect(() => {
    const setupConnection = async () => {
      try {
        const user = auth.currentUser;

        if (user) {
          const token = await user.getIdToken(true); // Get the JWT token

          const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:5232/chat", {
              accessTokenFactory: () => token,
              withCredentials: true,
            })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();

          newConnection.on("ReceiveMessage", (user, message) => {
            setMessages(messages => [...messages, { user, text: message, channel: currentChannel, timestamp: new Date().toLocaleTimeString() }]);
          });

          newConnection.onreconnected(() => {
            console.log("Reconnected to the SignalR hub");
          });

          newConnection.onclose(() => {
            console.log("Connection closed");
          });

          await newConnection.start();
          console.log("Connected to SignalR hub");

          await newConnection.invoke("JoinRoom", currentChannel);

          setConnection(newConnection);
        }
      } catch (e) {
        console.log("Connection failed: ", e);
      }
    };

    setupConnection();

    return () => {
      if (connection) {
        connection.stop();
      }
    };
  }, [currentChannel]);

  const handleSendMessage = async () => {
    if (connection && connection.connectionStarted) {
      try {
        await connection.invoke("SendMessage", currentChannel, auth.currentUser.email, message);
        setMessage('');
      } catch (e) {
        console.error("Message send failed: ", e);
      }
    } else {
      console.error("No connection to server yet.");
      // Retry sending the message after a brief delay
      setTimeout(() => handleSendMessage(), 500);
    }
  };

  const handleChannelChange = async (channel) => {
    if (connection) {
      await connection.invoke("LeaveRoom", currentChannel);
      await connection.invoke("JoinRoom", channel);
      setCurrentChannel(channel);
      setMessages([]); // Optionally clear messages or load new ones for the channel
    }
  };

  return (
    <div className="chat-box">
      <ChatHeader currentChannel={currentChannel} />
      <ChannelTabs currentChannel={currentChannel} onChannelChange={handleChannelChange} />
      <ChatMessages messages={messages.filter(msg => msg.channel === currentChannel)} />
      <ChatInput
        message={message}
        onMessageChange={setMessage}
        onSendMessage={handleSendMessage}
      />
    </div>
  );
};

const ChatHeader = ({ currentChannel }) => (
  <div className="chat-header">
    <h3>{currentChannel}</h3>
  </div>
);

const ChannelTabs = ({ currentChannel, onChannelChange }) => (
  <div className="channel-tabs">
    {['general', 'local', 'moderator', 'admin', 'owner'].map((channel) => (
      <button
        key={channel}
        className={currentChannel === channel ? 'active' : ''}
        onClick={() => onChannelChange(channel)}
      >
        {channel}
      </button>
    ))}
  </div>
);

const ChatMessages = ({ messages }) => (
  <div className="chat-messages">
    {messages.map((msg, index) => (
      <div key={index} className="chat-message">
        <strong>{msg.user}</strong> [{msg.timestamp}]: {msg.text}
      </div>
    ))}
  </div>
);

const ChatInput = ({ message, onMessageChange, onSendMessage }) => (
  <div className="chat-input">
    <input
      type="text"
      value={message}
      onChange={(e) => onMessageChange(e.target.value)}
      onKeyPress={(e) => {
        if (e.key === 'Enter') {
          onSendMessage();
        }
      }}
    />
    <button onClick={onSendMessage}>Send</button>
  </div>
);

export default ChatBox;
