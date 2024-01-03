import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscribable, Subscription, switchMap } from 'rxjs';
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
  piecesMovedSubscription?: Subscription;
  @Input() type!: string;
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

    if (this.piecesMovedSubscription == undefined) {
      this.piecesMovedSubscription = this.gameService.piecesMoved$.subscribe((piecesMoved) => {
        this.newPiecesEvent(piecesMoved);
      });
    }

    this.gameService.canMovePiece$.subscribe((movePiece) => {
      this.canMovePiece = movePiece;
    });
  }

  private startGamePieces() {
    this.game.players.forEach((player) => {
      this.newPiecesEvent(player.pieces);
    });
  }

  private newPiecesEvent(pieces: Piece[]) {
    pieces.forEach((piece) => {
      if (piece.nextPosition === this.position) {
        this.pieces.push(piece);
      }
      if (piece.previousPosition === this.position) {
        const indexToRemove = this.pieces.findIndex(p => p.color === piece.color);

        if (indexToRemove !== -1) {
          this.pieces.splice(indexToRemove, 1);
        }
      }
    })
  }

  movePiece(): void {
    //this.numberOfPieces++;
    if (this.canMovePiece) {
      let player = this.game.players.filter((p) => p.name === this.gameService.currentUser?.username)[0];

      let playerColor = player.pieces[0].color;

      let hasPlayerPieceOnThisCell: boolean = false;

      hasPlayerPieceOnThisCell = this.pieces.some((piece) => {
        return piece.color === playerColor;
      });

      if (hasPlayerPieceOnThisCell) {
        this.gameService.movePiece(this.position, playerColor);
      }
    }
  }

  togglePiece(): void {
    this.numberOfPieces++;
  }

  getArray(value: number): number[] {
    return new Array(value);
  }

  getPieceSize(): number {
    let numberOfPieces = this.pieces.length;

    if (numberOfPieces > 4) {
      return 50;
    }

    const baseSize = 70; // Change this to your desired base size
    const minSize = 40; // Change this to your desired minimum size
    const sizeDecrement = (baseSize - minSize) / 15; // Adjust based on the maximum number of pieces

    return Math.max(minSize, baseSize - sizeDecrement * (Math.min(numberOfPieces, 16) - 1));
  }

  getLeftPosition(index: number): number {
    let numberOfPieces = this.pieces.length;

    switch (numberOfPieces) {
      case 1:
        return 60;
      case 2:
        return 35 + index * 50;
      case 3:
        return 35 + (index % 2) * 50 + Math.floor(index / 2) * 25;
      case 4:
        return 35 + (index % 2) * 50;
      default:
        return 25 + (index % 4) * 25;
    }
  }

  getTopPosition(index: number): number {
    let numberOfPieces = this.pieces.length;

    switch (numberOfPieces) {
      case 1:
        return 60;
      case 2:
        return 60;
      case 3:
        return 35 + Math.floor(index / 2) * 50;
      case 4:
        return 35 + Math.floor(index / 2) * 50;
      default:
        return 25 + Math.floor(index / 4) * 25;
    }
  }

  getCellType(cellType: string) {
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
      id: 0,
      players: [],
      firstDiceRoller: ''
    };

    return game;
  }

  ngOnDestroy(): void {
    if (this.piecesMovedSubscription != undefined) {
      this.piecesMovedSubscription.unsubscribe();
    }
  }
}
