import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { GameService } from 'src/app/services/game.service';
import { Game } from 'src/app/shared/entities/game';
import { Piece } from 'src/app/shared/entities/piece';
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
  pieces: Piece[] = [];
  canMovePiece: boolean = false;
  @Input() color!: ColorType;
  @Input() position!: number;

  redColor: ColorType = ColorType.Red;
  blueColor: ColorType = ColorType.Blue;
  greenColor: ColorType = ColorType.Green;
  yellowColor: ColorType = ColorType.Yellow;

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
        this.startGamePieces();
      }
    });

    this.gameService.piecesMoved$.subscribe((piecesMoved) => {
      this.newPiecesEvent(piecesMoved);
    });

    this.gameService.canMovePiece$.subscribe((movePiece)=>{
      this.canMovePiece = movePiece;
    });
  }

  private startGamePieces(){
    this.game.players.forEach((player)=>{
      this.newPiecesEvent(player.pieces);
    });
  }

  private newPiecesEvent(pieces: Piece[]){
    pieces.forEach((piece) => {
      if (piece.nextPosition === this.position) {
          this.pieces.push(piece);            
      }
      if(piece.previousPosition === this.position){
        this.pieces.filter(piece => piece!== piece);
      }
    })
  }

  // TEST FUNCTION TO DISPLAY PIECES ON A CELL
  movePiece(): void {
    //this.numberOfPieces++;
    if(this.canMovePiece){
      let player = this.game.players.filter((p)=>p.name===this.gameService.currentUser?.username)[0];

      let playerColor = player.pieces[0].color;

      let hasPlayerPieceOnThisCell: boolean = false;

      hasPlayerPieceOnThisCell = player.pieces.some((piece) => {
        return piece.color === playerColor;
      });

      if(hasPlayerPieceOnThisCell){
        this.gameService.movePiece(this.position);
      }
    }
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
      id: 0,
      players: [],
      firstDiceRoller: ''
    };

    return game;
  }
}
