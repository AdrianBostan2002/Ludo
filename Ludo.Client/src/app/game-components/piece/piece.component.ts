import { Component, Input, Output } from '@angular/core';
import { ColorType } from 'src/app/shared/enums/color-type';

@Component({
  selector: 'app-piece',
  templateUrl: './piece.component.html',
  styleUrls: ['./piece.component.css']
})
export class PieceComponent {
  @Input() color!: ColorType;
  @Input() pieceSize!: number;

  getPieceColor(colorType: ColorType) {
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
}
