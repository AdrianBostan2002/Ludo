import { Component, OnInit } from '@angular/core';
import { GameService } from '../services/game.service';
import { Game } from '../shared/entities/game';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { ColorType } from '../shared/enums/color-type';
import { getColorString } from '../shared/utils/color-type-converter';
import { IconDefinition, faChessPawn, faCircle, faClose, faDotCircle, faFaceMehBlank, faFilm, faLineChart, faLink, faList, faPersonWalkingDashedLineArrowRight } from '@fortawesome/free-solid-svg-icons';
import { IconProp } from '@fortawesome/fontawesome-svg-core';

interface Piece {
  position: number,
  color: ColorType;
}

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {
  filmIcon = faChessPawn;

  game: Game = this.initializeEmptyGame();
  currentGameId: Number = 0;
  currentDirection: string = "down";

  lastPiece: Piece={color: ColorType.White, position:0};

  pieces: Piece[] = [
    { position: 1, color: ColorType.Blue}, {position: 1, color: ColorType.Red}, {position: 5, color: ColorType.Green}
  ];

  defaultEmptyIcon: IconDefinition = faCircle;
  handleButtonClick(button: any) {
    // Handle button click logic here
  }


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

  getColorString(color: ColorType): any {
    return getColorString(color);
  }

  // getCellDirection(cell: any): { [key: string]: string } {
  //   // Implement your logic to determine cell direction
  //   const direction = cell.type === CellType.Special ? 'vertical' : 'horizontal'; // Replace with your logic



  getIcon(position: number): IconProp {
    const piece = this.pieces.find((p) => p.position === position);

    if (piece?.color === ColorType.Blue || piece?.color == ColorType.Red || piece?.color == ColorType.Green || piece?.color == ColorType.Yellow) {
      this.lastPiece = piece;    
      return this.filmIcon;
      }

    return faClose; // Default icon if no piece found for the button
  }

  getIconClasses(position: number): string[] {
    const piece = this.pieces.find((p) => p.position === position);

    switch (piece?.color) {
      case ColorType.Blue:
        return ['blue-icon'];
      case ColorType.Red:
        return ['red-icon'];
      case ColorType.Green:
        return ['green-icon'];
      // Add cases for other colors
      default:
        return [];
    }
  }

}
