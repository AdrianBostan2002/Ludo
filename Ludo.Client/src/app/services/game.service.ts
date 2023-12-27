import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { StartGameSuccesfullyResponse } from '../shared/entities/start-game-sucessfully-response';
import { Game } from '../shared/entities/game';
import { BehaviorSubject, Subject } from 'rxjs';
import { User } from '../shared/interfaces/user.interface';
import { Router } from '@angular/router';
import { PieceMoved } from '../shared/entities/piece-moved';
import { ColorType } from '../shared/enums/color-type';
import { Piece } from '../shared/entities/piece';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  connectionUrl = `https://localhost:7192/gameHub`;

  lobbyId: number = 0;
  currentUser?: User;
  currentGame!: Game;
  lastDiceNumber: Number = 0;

  private hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(this.connectionUrl)
    .build();

  game$: BehaviorSubject<Game | undefined> = new BehaviorSubject<Game | undefined>(undefined);
  canRoleDice$: Subject<boolean> = new Subject<boolean>();
  canMovePiece$: Subject<boolean> = new Subject<boolean>();
  diceNumber$: Subject<number> = new Subject<number>();
  piecesMoved$: Subject<Piece[]> = new Subject<Piece[]>();
  newReadyPlayer$: Subject<string> = new Subject<string>();
  isReady$: BehaviorSubject<boolean> = new BehaviorSubject(false);
  
  constructor(private router: Router) {
    window.addEventListener('beforeunload', (event: Event): void => {
      if (this.lobbyId !== 0 && this.currentUser !== undefined) {
        this.playerLeave(this.lobbyId, this.currentUser);
      }
    });
  }

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
    this.currentUser = user;
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

  public async playerLeave(lobbyId: number, player: User) {
    await this.checkConnection();

    this.hubConnection.send("PlayerLeave", Number(lobbyId), player.username);
  }

  public async checkConnection(): Promise<void> {
    try {
      if (this.hubConnection.state === signalR.HubConnectionState.Disconnected) {
        console.log("Connecting........")
        await this.startConnection();
      }

    } catch (error) {
      console.error('Error during connection or sending signal:', error);
    }
  }

  public roleDice = async (gameId: number) => {
    await this.checkConnection();

    this.hubConnection.send("RollDice", Number(gameId));
  }

  public movePiece = async (position: number, pieceColor: ColorType) =>{
    await this.checkConnection();

    let piece: Piece = {color: pieceColor, previousPosition: position };

    this.hubConnection.send("MovePiece", this.currentUser?.username, Number(this.lobbyId), piece, this.lastDiceNumber);
  }

  public addGameListener = () => {
    this.hubConnection.on('PreprocessingSuccessfully', (data) => {
      console.log('PreprocessingSuccessfully');
      console.log(data);
      data.forEach((readyUser: any) => {
        this.newReadyPlayer$.next(readyUser);
      });
    });

    this.hubConnection.on('StartGameSucceded', (data: Game) => {
      console.log('data.game:', data);
      this.game$.next(data);
      this.currentGame = data;
      this.router.navigate([`game/${this.lobbyId}`]);
    });

    this.hubConnection.on('GameStarted', (data: Game) => {
      console.log('data.game:', data);
      this.game$.next(data);
      this.currentGame = data;
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

    this.hubConnection.on('LeavingSucceeded', () => {

      this.disconnectFromHub();
      //this.router.navigate(['']);
      console.log("Leaving Game Succeeded");

    });

    this.hubConnection.on('PlayerLeftGame', (data) => {
      console.log(`${data} left game`);
    });

    this.hubConnection.on('DiceRolled', (data) => {
      console.log(`Dice rolled: ${data}`);

      if(data.canMovePieces)

      this.lastDiceNumber = data.diceNumber;
      this.diceNumber$.next(data.diceNumber);
      this.canMovePiece$.next(data.canMovePieces)
    });

    this.hubConnection.on('CanRollDice', ()=>{
      this.canRoleDice$.next(false);
    });

    this.hubConnection.on('PiecesMoved', (data)=>{
      let piecesMoved: Piece[] = data;
      this.canMovePiece$.next(false);
      this.piecesMoved$.next(piecesMoved);
    });
  }

  public disconnectFromHub() {
    this.hubConnection.stop();
  }
}

