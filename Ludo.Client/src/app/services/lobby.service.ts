import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { User } from '../shared/interfaces/user.interface';
import { RoleType } from '../shared/enums/roletype.enum';

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

  public participantLeave(lobbyId: Number, lobbyParticipant: User) {
    if (this.hubConnection.state == signalR.HubConnectionState.Connected) {
      this.hubConnection.send('ParticipantLeave', Number(lobbyId), lobbyParticipant.username);
    }
  }

  public addLobbyListener = () => {
    this.hubConnection.on('NewUserJoined', (data) => {
      this.data = data;
      this.lobbyParticipants.push(data.username);
      console.log(this.lobbyParticipants);
    });

    this.hubConnection.on('JoinedLobby', (data) => {
      this.data = data;
      this.lobbyId$.next(data.lobbyId);
      this.lobbyParticipants.push(data.username);
      console.log(data);
      this.router.navigate(['/lobby', data.lobbyId]);
    });

    this.hubConnection.on('SuccessfullyContectedToLobby', (data) => {
      data.lobbyParticipants.forEach((element: any) => {
        console.log(element);
        this.lobbyParticipants.push(element.name);
        console.log(this.lobbyParticipants);
      });

      this.router.navigate(['/lobby', data.lobbyId]);
    });

    this.hubConnection.on('UnSuccessfullyContectedToLobby', (data) => {
      console.log('Unsuccessfully Contected to Lobyy', data);
    });

    this.hubConnection.on('LeaveLobbySucceeded', () => {

      this.disconnectFromHub();
      this.router.navigate(['']);
      console.log("Leave Lobby Succeeded");
    });

    this.hubConnection.on('LeaveLobbyFailed', () => {
      this.router.navigate(['']);
      console.log("Left Lobby Successfully");
    });

    this.hubConnection.on('PlayerLeftLobby', (data) => {
      const indexToRemove = this.lobbyParticipants.findIndex(participant => participant === data);

      if (indexToRemove !== -1) {
        this.lobbyParticipants.splice(indexToRemove, 1);
      }

      console.log(`${data} left lobby`);
    });
  }

  public disconnectFromHub() {
    this.hubConnection.stop();
  }
}
