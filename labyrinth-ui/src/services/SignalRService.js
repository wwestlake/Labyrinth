// SignalRService.js

import * as signalR from '@microsoft/signalr';

class SignalRService {
  constructor() {
    this.connection = null;
  }

  async startConnection() {
    const token = this.getToken();

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/chat', {
        accessTokenFactory: () => token,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Trace) 
      .build();

    try {
      await this.connection.start();
      console.log('Connected to SignalR Hub');
    } catch (err) {
      console.error('Error connecting to SignalR Hub:', err);
    }

    console.log('SignalR connection state after start:', this.connection.state);
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

    console.log('Current SignalR connection state:', this.connection.state); // Add this line

    if (this.connection.state !== signalR.HubConnectionState.Connected) {
        console.error('SignalR connection is not connected.');
        return;
    }

    try {
        console.log(`Sending message to ${channelId} from ${user}: ${message}`);
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
    return localStorage.getItem('token');  // Example: replace with your token retrieval logic
  }
}

const signalRService = new SignalRService();
export default signalRService;
