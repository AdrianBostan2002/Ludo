import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, switchMap } from 'rxjs';
import { GameService } from 'src/app/services/game.service';
import { Game } from 'src/app/shared/entities/game';

@Component({
  selector: 'app-dice-roll',
  templateUrl: './dice-roll.component.html',
  styleUrls: ['./dice-roll.component.css']
})
export class DiceRollComponent {
  gameId: number = 0;
  currentGame?: Game;
  canRollDice: boolean = true;

  canRoleDiceSubscription?: Subscription;
  diceNumberSubscription?: Subscription;

  constructor(private gameService: GameService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.currentGame = this.gameService.currentGame;
    if (this.currentGame.firstDiceRoller === this.gameService.currentUser?.username) {
      this.canRollDice = false;
    }

    this.gameId = this.gameService.lobbyId;

    this.canRoleDiceSubscription = this.gameService.canRoleDice$.subscribe((canRoleDice) => {
      this.canRollDice = canRoleDice;
    })

    this.diceNumberSubscription = this.gameService.diceNumber$.subscribe((diceNumber) => {
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
      this.canRollDice = true;
      //this.gameService.canMovePiece$.next(true);
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

  ngOnDestroy(): void {
    if (this.canRoleDiceSubscription!=undefined) {
      this.canRoleDiceSubscription.unsubscribe();
    }
    if(this.diceNumberSubscription!=undefined){
      this.diceNumberSubscription;
    }
  }
}