import { ColorType } from "../enums/color-type";

export function getColorString(color: ColorType): string {
    switch (color) {
      case ColorType.Blue:
        return 'blue';
      case ColorType.Red:
        return 'red';
      case ColorType.Green:
        return 'green';
      case ColorType.Yellow:
        return 'yellow';
      case ColorType.White:
        return 'white';
      default:
        return ''; 
    }
}


export function stringToEnum<T>(enumObj: any, value: string): T {
  return enumObj[value as keyof typeof enumObj] as T;
}