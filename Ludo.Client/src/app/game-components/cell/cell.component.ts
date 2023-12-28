import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { GameService } from 'src/app/services/game.service';
import { Game } from 'src/app/shared/entities/game';
import { ColorType } from 'src/app/shared/enums/color-type';

@Component({
  selector: 'app-cell',
  templateUrl: './cell.component.html',
  styleUrls: ['./cell.component.css']
})
export class CellComponent {
  game: Game = this.initializeEmptyGame();
  currentGameId: number = 0;
  numberOfPieces: number = 0;
  @Input() type!: string;
  @Input() color!: ColorType;

  redColor: ColorType = ColorType.Red;
  blueColor: ColorType = ColorType.Blue;
  greenColor: ColorType = ColorType.Green;
  yellowColor: ColorType = ColorType.Yellow;

  constructor(private gameService: GameService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    // this.route.params.pipe(
    //   switchMap(params => {
    //     this.currentGameId = params['gameId'];
    //     return this.gameService.game$;
    //   })
    // ).subscribe(game => {
    //   if (game != undefined) {
    //     this.game = game;
    //   }
    //   console.log('current game:', this.game)
    // });
  }

  // TEST FUNCTION TO DISPLAY PIECES ON A CELL
  togglePiece(): void {
    this.numberOfPieces++;
  }

  getArray(value: number): number[] {
    return new Array(value);
  }

  getLeftPosition(index: number): number {
    return 10 + (index % 4) * 25;
  }

  getTopPosition(index: number): number {
    return 10 + Math.floor(index / 4) * 25;
  }

  getCellType(cellType: string) {
    //return [cellType];
    switch (cellType) {
      case 'green-icon':
        return ['green-icon'];
      case 'red-icon':
        return ['red-icon'];
      case 'blue-icon':
        return ['blue-icon'];
      case 'yellow-icon':
        return ['yellow-icon'];
      case 'green-home':
        return ['green-home'];
      case 'red-home':
        return ['red-home'];
      case 'blue-home':
        return ['blue-home'];
      case 'yellow-home':
        return ['yellow-home'];
      case 'green-final':
        return ['green-final'];
      case 'red-final':
        return ['red-final'];
      case 'blue-final':
        return ['blue-final'];
      case 'yellow-final':
        return ['yellow-final'];
      default:
        return [];
    }
  }

  getCellColor(colorType: ColorType) {
    switch (colorType) {
      case ColorType.Blue:
        return ['blue-icon'];
      case ColorType.Red:
        return ['red-icon'];
      case ColorType.Green:
        return ['green-icon'];
      case ColorType.Yellow:
        return ['yellow-icon'];
      default:
        return [];
    }
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
