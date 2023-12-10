import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dice-roll',
  templateUrl: './dice-roll.component.html',
  styleUrls: ['./dice-roll.component.css']
})
export class DiceRollComponent {

  rollDice() {
    const dice = document.querySelectorAll(".die-list");
    dice.forEach((die: any) => {
      this.toggleClasses(die);
      (die as HTMLElement).dataset['roll'] = this.getRandomNumber(1, 6).toString();
    });
  }

  toggleClasses(die: HTMLElement) {
    die.classList.toggle("odd-roll");
    die.classList.toggle("even-roll");
  }

  getRandomNumber(min: number, max: number) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }


}
