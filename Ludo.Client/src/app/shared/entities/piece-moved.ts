import { Piece } from "./piece";

export interface PieceMoved{
    piece: Piece;
    previousPosition?: number;
    nextPosition: number;
}