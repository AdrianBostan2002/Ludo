import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as signalR from '@microsoft/signalr';
import { StartGameSuccesfullyResponse } from '../shared/entities/start-game-sucessfully-response';
import { Game } from '../shared/entities/game';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  private hubConnection: signalR.HubConnection;

  game$: Subject<Game>= new Subject<Game>();

  constructor(private router: Router) { }

  public startGame = (lobbyId: number) => {

    //let lobbyId: Number = this.route.snapshot.data['lobbyId'];
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7192/gameHub')
      .build();
      
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.hubConnection.send("StartGame", lobbyId);
      })
      .catch(err => console.log('Error while starting connection: ' + err))

      this.hubConnection.onclose((error) => {
        console.error('Connection closed with an error:', error);
      });
      
      this.hubConnection.onreconnecting((error) => {
        console.warn('Connection is reconnecting:', error);
      });
  }

  public addGameListener = () => {
    this.hubConnection.on('StartGameSucceded', (data: StartGameSuccesfullyResponse) => {
      this.router.navigate(['/board']);
      console.log(data);
      this.game$.next(data.game);
    });

    this.hubConnection.on('StartGameFailed', (data) => {
      data.forEach((element: any) => {
        console.log(element);
      });
    });
  }
}