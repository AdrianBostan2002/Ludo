import { Component, OnInit } from '@angular/core';
import { GameService } from '../services/game.service';
import { Game } from '../shared/entities/game';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { ColorType } from '../shared/enums/color-type';
import { getColorString } from '../shared/utils/color-type-converter';
import { CellType } from '../shared/enums/cell-type';
import { Cell } from '../shared/entities/cell';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  game: Game = this.initializeEmptyGame();
  currentGameId: Number = 0;
  currentDirection: string = "down";
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
      board: { cells: [], finalCells: []},
      id: 0,
      players: []
    };

    return game;
  }

  getColorString(color: ColorType): any {
    return getColorString(color);
  }

  // getCellDirection(cell: any): { [key: string]: string } {
  //   // Implement your logic to determine cell direction
  //   const direction = cell.type === CellType.Special ? 'vertical' : 'horizontal'; // Replace with your logic


  //   // switch (direction) {
  //   //   case 'up':
  //   //     return { 'transform': 'rotate(0deg)' };
  //   //   case 'down':
  //   //     return { 'transform': 'rotate(180deg)' };
  //   //   case 'left':
  //   //     return { 'transform': 'rotate(-90deg)' };
  //   //   case 'right':
  //   //     return { 'transform': 'rotate(90deg)' };
  //   //   default:
  //   //     return {};
  //   // }
  // }
}
