import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { BoardComponent } from 'src/app/board/board.component';
import { GameService } from 'src/app/services/game.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { Game } from 'src/app/shared/entities/game';
import { ColorType } from 'src/app/shared/enums/color-type';
import { User } from 'src/app/shared/interfaces/user.interface';

@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.css']
})
export class GamePageComponent {

  private currentGameId: number = 0;
  public currentGameParticipant !: User;
  private currentGame!: Game | undefined;
  public userColor!: ColorType;
  randomDiceNumber: string = '';
  isGameOver: boolean = true;

  constructor(private gameService: GameService, private lobbyService: LobbyService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    // this.route.params.subscribe(params => {
    //   this.currentGameId = params['gameId'];
    // });

    this.route.params.pipe(
      switchMap(params => {
        this.currentGameId = params['gameId'];
        return this.gameService.game$; // Switch to the game$ observable
      })
    ).subscribe(game => {
      this.currentGame = game;
      console.log('current game:', this.currentGame);

    });

    //this.gameService.game$.subscribe(game => {this.currentGame = game; console.log('game:', game)});
    //this.currentGame = this.gameService.currentGame;

    //console.log('current game:', this.currentGame);
    this.currentGameParticipant = this.lobbyService.currentLobbyParticipant;

    let pieces = this.currentGame?.players.find(obj => obj.name == this.currentGameParticipant?.username)?.pieces;
    this.userColor = pieces?.[0]?.color!;
  }

  getPlayerColorClass(color: ColorType) {
    switch (color) {
      case ColorType.Blue:
        return 'blue-text';
      case ColorType.Yellow:
        return 'yellow-text';
      case ColorType.Red:
        return 'red-text';
      case ColorType.Green:
        return 'green-text';
      default:
        return '';
    }
  }

  getPlayerColorText(color: ColorType) {
    switch (color) {
      case ColorType.Blue:
        return 'BLUE';
      case ColorType.Yellow:
        return 'YELLOW';
      case ColorType.Red:
        return 'RED';
      case ColorType.Green:
        return 'GREEN';
      default:
        return '';
    }
  }
}
