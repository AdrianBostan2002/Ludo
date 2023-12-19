import { Component, OnInit } from '@angular/core';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'app-dice-roll',
  templateUrl: './dice-roll.component.html',
  styleUrls: ['./dice-roll.component.css']
})
export class DiceRollComponent {

  gameId: number=0;

  constructor(private gameService: GameService){}

  ngOnInit(): void {
    this.gameId = this.gameService.lobbyId;

    this.gameService.diceNumber$.subscribe((diceNumber)=>{
      this.diceRolled(diceNumber)
    });
  }

  rollDice() {
    this.gameService.roleDice(this.gameId);
  }

  diceRolled(diceNumber: Number) {
    const dice = document.querySelectorAll(".die-list");
    dice.forEach((die: any) => {
      this.toggleClasses(die);
      (die as HTMLElement).dataset['roll'] = diceNumber.toString();
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
