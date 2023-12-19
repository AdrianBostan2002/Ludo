import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { GameService } from 'src/app/services/game.service';
import { Game } from 'src/app/shared/entities/game';

@Component({
  selector: 'app-cell',
  templateUrl: './cell.component.html',
  styleUrls: ['./cell.component.css']
})
export class CellComponent {
  game: Game = this.initializeEmptyGame();
  currentGameId: Number = 0;

  constructor(private gameService: GameService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.pipe(
      switchMap(params => {
        this.currentGameId = params['gameId'];
        return this.gameService.game$;
      })
    ).subscribe(game => {
      if (game != undefined) {
        this.game = game;
      }
      console.log('current game:', this.game)
    });
  }

  private initializeEmptyGame(): Game {
    let game: Game = {
      board: { cells: [], finalCells: [] },
      id: 0,
      players: []
    };

    return game;
  }
}
