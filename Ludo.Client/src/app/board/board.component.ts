import { Component, OnInit } from '@angular/core';
import { GameService } from '../services/game.service';
import { Game } from '../shared/entities/game';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { ColorType } from '../shared/enums/color-type';
import { getColorString } from '../shared/utils/color-type-converter';
import { faChessPawn, faFilm } from '@fortawesome/free-solid-svg-icons';
import { Cell } from '../shared/entities/cell';
import { CellType } from '../shared/enums/cell-type';

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

  firstSetOfBoxes: Cell[] = [
    {color: ColorType.Red, pieces: [], type : CellType.Basic}, 
    {color: ColorType.Red, pieces: [], type : CellType.Basic},
    {color: ColorType.Red, pieces: [], type : CellType.Basic}
  ];

  

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


  handleButtonClick(event: MouseEvent) {
    const targetButton = event.target as HTMLButtonElement;

    // Check if the clicked button has an icon
    if (targetButton.querySelector('.icon')) {
      // Move the icon to another button
      this.moveIcon(targetButton);
    }
  }

  moveIcon(sourceButton: HTMLButtonElement) {
    // Find the target button (for example, next adjacent button)
    const targetButtonId = 'some-logic-to-find-target'; // Implement your logic here
    const targetButton = document.getElementById(targetButtonId);

    // Check if the target button exists
    if (targetButton) {
      // Get the icon from the source button
      const icon = sourceButton.querySelector('.icon');

      // Remove the icon from the source button
      //sourceButton.removeChild(icon);

      // Append the icon to the target button
      //targetButton.appendChild(icon);
    }
  }

//   const buttons = document.querySelectorAll('.piece-position');
// buttons.forEach((button) => {
//   button.addEventListener('click', handleButtonClick);
// });
}
