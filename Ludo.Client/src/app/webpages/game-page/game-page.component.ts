import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { GameService } from 'src/app/services/game.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { Game } from 'src/app/shared/entities/game';
import { User } from 'src/app/shared/interfaces/user.interface';

@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.css']
})
export class GamePageComponent {

  private currentGameId: number = 0;
  private currentGameParticipant !: User;
  private currentGame!: Game | undefined;

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
      console.log('current game:', this.currentGame)});

    //this.gameService.game$.subscribe(game => {this.currentGame = game; console.log('game:', game)});
    //this.currentGame = this.gameService.currentGame;

    //console.log('current game:', this.currentGame);

    this.currentGameParticipant = this.lobbyService.currentLobbyParticipant;

    this.gameService.addGameListener();
  }
}
