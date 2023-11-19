import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class LobbyService {
  lobbyParticipants: string[] = [];
  lobbyId: number = 0;

  data: string = '';

  private hubConnection: signalR.HubConnection;

  public createLobbyConnection = (username: string) => {
    const connectionUrl = `https://localhost:7192/lobbyHub`;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(connectionUrl)
      .build();
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.hubConnection.send("CreatedLobby", username);
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public joinLobbyConnection = (lobbyId: number, username: string) => {
    const connectionUrl = `https://localhost:7192/lobbyHub`;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(connectionUrl)
      .build();
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.hubConnection.send("JoinLobby", lobbyId, username);
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addLobbyListener = () => {
    this.hubConnection.on('NewUserJoined', (data) => {
      this.data = data;
      this.lobbyParticipants.push(data.username);
      this.lobbyId = data.randomLobbyId;
      console.log(data);
    });

    this.hubConnection.on('SuccessfullyContectedToLobby', (data) => {
      data.forEach((element: any) => {
        console.log(element);
        this.lobbyParticipants.push(element.name);
      });
    });

    this.hubConnection.on('UnSuccessfullyContectedToLobby', (data) => {
      console.log(data);
    });
  }
}
