import { ColorType } from "../enums/color-type";
import { Piece } from "./piece";

export interface Cell{
    color: ColorType,
    pieces: Piece[]
}