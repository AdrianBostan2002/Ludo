import { Component, OnInit } from '@angular/core';
import { GameService } from '../services/game.service';
import { Game } from '../shared/entities/game';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {
  // game: Game | undefined;

  // constructor(private gameService: GameService) { }

  ngOnInit(): void {
    // this.gameService.game$.subscribe(game=>{
    //   this.game = game;
    // })
  }

}
