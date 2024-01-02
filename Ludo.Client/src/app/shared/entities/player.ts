import { Piece } from "./piece";

export interface Player{
    name: string,
    pieces: Piece[],
    isReady: boolean
}