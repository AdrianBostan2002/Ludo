import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameService } from 'src/app/services/game.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { RoleType } from 'src/app/shared/enums/roletype.enum';
import { User } from 'src/app/shared/interfaces/user.interface';

@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.css']
})
export class GamePageComponent {

  private currentGameId: number = 0;
  private currentGameParticipant !: User;

  constructor(private gameService: GameService, private lobbyService: LobbyService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.currentGameId = params['gameId'];
    });

    this.currentGameParticipant = this.lobbyService.currentLobbyParticipant;

    this.gameService.addGameListener();
  }
}
