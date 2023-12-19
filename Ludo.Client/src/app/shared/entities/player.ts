import { Piece } from "./piece";

export interface Player{
    name: string,
    connectionId: string,
    pieces: Piece[],
    isReady: boolean
}