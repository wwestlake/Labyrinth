// SignalRService.js

import * as signalR from '@microsoft/signalr';

class SignalRService {
  constructor() {
    this.connection = null;
  }

  async startConnection() {
    // Retrieve the token (replace with your actual logic)
    const token = this.getToken();

    // Set up the SignalR connection with authorization header
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5232/chat', {
        accessTokenFactory: () => token,  // Attach the token here
      })
      .withAutomaticReconnect()
      .build();

    // Start the connection and handle any errors
    try {
      await this.connection.start();
      console.log('Connected to SignalR Hub');
    } catch (err) {
      console.error('Error connecting to SignalR Hub:', err);
    }
  }

  stopConnection() {
    if (this.connection) {
      this.connection.stop();
    }
  }

  async sendMessage(channelId, user, message) {
    if (!this.connection) {
      console.error('No SignalR connection available.');
      return;
    }

    try {
      await this.connection.invoke('SendMessage', channelId, user, message);
    } catch (err) {
      console.error('Error sending message:', err);
    }
  }

  onReceiveMessage(callback) {
    if (!this.connection) {
      console.error('No SignalR connection available.');
      return;
    }

    this.connection.on('ReceiveMessage', (user, message) => {
      callback(user, message);
    });
  }

  offReceiveMessage() {
    if (this.connection) {
      this.connection.off('ReceiveMessage');
    }
  }

  // Function to retrieve token from storage (replace with actual implementation)
  getToken() {
    return localStorage.getItem('userToken');  // Example: replace with your token retrieval logic
  }
}

const signalRService = new SignalRService();
export default signalRService;
