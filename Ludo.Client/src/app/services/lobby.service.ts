import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { User } from '../shared/interfaces/user.interface';
import { RoleType } from '../shared/enums/roletype.enum';
import { StartGameSuccesfullyResponse } from '../shared/entities/start-game-sucessfully-response';

@Injectable({
  providedIn: 'root'
})
export class LobbyService {
  lobbyParticipants: string[] = [];
  lobbyId$: Subject<number> = new Subject<number>();
  currentLobbyParticipant: User = new User();
  data: string = '';
  connectionUrl = `https://localhost:7192/lobbyHub`;

  private hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(this.connectionUrl)
    .build();

  constructor(private router: Router) { }

  public createLobbyParticipant(username: string, role: RoleType): void {
    this.currentLobbyParticipant.username = username;
    this.currentLobbyParticipant.role = role;
  }

  public createLobbyConnection = (username: string) => {
    this.hubConnection
      .start()
      .then(() => { 
        console.log('Connection started');
        this.hubConnection.send("CreatedLobby", username);
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public joinLobbyConnection = (lobbyId: number, username: string) => {
    if (this.hubConnection.state === signalR.HubConnectionState.Disconnected) {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection started');
          this.hubConnection.send("JoinLobby", lobbyId, username);
        })
        .catch(err => console.log('Error while starting connection: ' + err));
    } else {
      this.hubConnection.send("JoinLobby", lobbyId, username);
    }
  }

  public addLobbyListener = () => {
    this.hubConnection.on('NewUserJoined', (data) => {
      this.data = data;
      //this.lobbyParticipants.push(this.currentLobbyParticipant);
      this.lobbyParticipants.push(data.username);
      console.log(this.lobbyParticipants);
      console.log(data);
      console.log(data);
    });

    this.hubConnection.on('JoinedLobby', (data) => {
      this.data = data;
      this.lobbyId$.next(data.lobbyId);
      //this.lobbyParticipants.push(this.currentLobbyParticipant);
      //this.lobbyParticipants.push(this.currentLobbyParticipant);
      console.log(data);
      this.router.navigate(['/lobby', data.lobbyId]);
    });

    this.hubConnection.on('SuccessfullyContectedToLobby', (data) => {
      data.lobbyParticipants.forEach((element: any) => {
        console.log(element);
        this.lobbyParticipants.push(element.name);
        //this.lobbyParticipants.push(this.currentLobbyParticipant);
        console.log(this.lobbyParticipants);
      });

      this.router.navigate(['/lobby', data.lobbyId]);
    });

    this.hubConnection.on('UnSuccessfullyContectedToLobby', (data) => {
      console.log(data);
    });

    this.hubConnection.on('GameStarted', (data) => {
      console.log(data);
    });
  }

  public DisconnectFromHub(){
    this.hubConnection.stop();
  }
}
