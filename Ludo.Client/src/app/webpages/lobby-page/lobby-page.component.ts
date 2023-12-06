import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from 'src/app/services/game.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { User } from 'src/app/shared/interfaces/user.interface';

@Component({
  selector: 'app-lobby-page',
  templateUrl: './lobby-page.component.html',
  styleUrls: ['./lobby-page.component.css']
})
export class LobbyPageComponent {
  lobbyParticipants: string[] = [];
  readyParticipants: string[] = [];
  currentLobbyId: number = 0;
  currentLobbyParticipant!: User;

  isReadyButtonEnabled: boolean = true;

  constructor(public lobbyService: LobbyService, private gameService: GameService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.currentLobbyId = params['lobbyId'];
    });
    this.lobbyParticipants = this.lobbyService.lobbyParticipants;
    this.currentLobbyParticipant = this.lobbyService.currentLobbyParticipant;

    this.gameService.newReadyPlayer$.subscribe(player => this.readyParticipants.push(player));
    this.gameService.isReady$.subscribe((status) => {
      if (status) {
        this.readyParticipants.push(this.currentLobbyParticipant.username)
      }
      this.isReadyButtonEnabled = !status;
    });

    this.gameService.startGamePreprocessing(this.currentLobbyId, this.currentLobbyParticipant);
    this.gameService.addGameListener();
  }

  getProfileInfo(userIndex: number) {
    return this.lobbyParticipants[userIndex] == this.lobbyService.currentLobbyParticipant.username ? 'your-user-profile' : 'user-profile';
  }

  startGame(): void {
    this.gameService.startGame(this.currentLobbyId);
  }

  leaveGame(): void {
    this.router.navigate(['']);
  }

  readyPlayer(): void {
    this.gameService.playerReady(this.currentLobbyId, this.currentLobbyParticipant);
  }
}
