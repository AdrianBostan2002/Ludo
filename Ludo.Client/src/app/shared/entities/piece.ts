import { ColorType } from "../enums/color-type";

export interface Piece{
    color: ColorType,
    nextPosition: number,
    previousPosition?: number
}