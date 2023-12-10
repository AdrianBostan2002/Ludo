import { ColorType } from "../enums/color-type";
import { Piece } from "./piece";
import { CellType } from "../enums/cell-type"; 

export interface Cell {
    color: ColorType,
    pieces: Piece[],
    type: CellType
}