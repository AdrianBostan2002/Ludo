import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Piece } from '../shared/entities/piece';
import { Player } from '../shared/entities/player';
import { GameService } from '../services/game.service';

@Component({
  selector: 'app-winning-modal',
  templateUrl: './winning-modal.component.html',
  styleUrls: ['./winning-modal.component.css']
})
export class WinningModalComponent {
  //@Input() name!: string;
  @Input() players!: Player[];

  constructor(private router: Router, private gameService: GameService) { }

  homePageRedirect() {
    this.router.navigate(['']);
  }
}
