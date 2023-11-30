import { Board } from "./board";
import { Player } from "./player";

export interface Game{
    id: Number,
    board: Board,
    players: Player[]
}