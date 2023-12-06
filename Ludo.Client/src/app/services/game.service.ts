import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { StartGameSuccesfullyResponse } from '../shared/entities/start-game-sucessfully-response';
import { Game } from '../shared/entities/game';
import { BehaviorSubject, Subject } from 'rxjs';
import { User } from '../shared/interfaces/user.interface';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  connectionUrl = `https://localhost:7192/gameHub`;

  private lobbyId: number = 0;

  private hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(this.connectionUrl)
    .build();

  game$: Subject<Game> = new Subject<Game>();

  newReadyPlayer$: Subject<string> = new Subject<string>();

  isReady$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(private router: Router) { }

  public startConnection = (): Promise<void> => {
    return new Promise<void>((resolve, reject) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection started');
          resolve(); // Resolve the promise when the connection is established
        })
        .catch((error) => {
          console.error('Error while starting connection:', error);
          reject(error); // Reject the promise if there's an error during connection
        });

      this.hubConnection.onclose((error) => {
        console.error('Connection closed with an error:', error);
        reject(error);
        // Reject the promise if the connection is closed with an error
      });

      this.hubConnection.onreconnecting((error) => {
        console.warn('Connection is reconnecting:', error);
      });
    });
  };

  public async startGamePreprocessing(lobbyId: number, user: User) {
    this.lobbyId = lobbyId;
    await this.checkConnection();

    this.hubConnection.send("StartGamePreprocessing", Number(lobbyId), user.username);
  }

  public async playerReady(lobbyId: number, user: User) {
    await this.checkConnection();

    this.hubConnection.send('ReadyToStartGame', Number(lobbyId), user.username);
  }

  public async startGame(lobbyId: number) {
    await this.checkConnection();

    this.hubConnection.send("StartGame", Number(lobbyId));
  }

  public async checkConnection(): Promise<void> {
    try {
      if (this.hubConnection.state === signalR.HubConnectionState.Disconnected) {
        await this.startConnection();
      }

    } catch (error) {
      console.error('Error during connection or sending signal:', error);
    }
  }

  public addGameListener = () => {
    this.hubConnection.on('PreprocessingSuccessfully', (data) => {
      console.log('PreprocessingSuccessfully');
      console.log(data);
      data.forEach((readyUser: any) => {
        this.newReadyPlayer$.next(readyUser);
      });
    });

    this.hubConnection.on('StartGameSucceded', (data: StartGameSuccesfullyResponse) => {
      console.log(data);
      this.router.navigate([`game/${this.lobbyId}`]);
      this.game$.next(data.game);
    });

    this.hubConnection.on('GameStarted', (data: StartGameSuccesfullyResponse) => {
      console.log(data);
      this.router.navigate([`game/${this.lobbyId}`]);
    });

    this.hubConnection.on('NewPlayerReady', (data) => {
      this.newReadyPlayer$.next(data);
      console.log(`New player ready: ${data}`);
    });

    this.hubConnection.on('ReadySuccessfully', () => {
      this.isReady$.next(true);

      console.log("Ready Successfully");
    });
  }
}

