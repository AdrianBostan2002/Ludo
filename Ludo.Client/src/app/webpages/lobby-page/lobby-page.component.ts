import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LobbyService } from 'src/app/services/lobby.service';
import { User } from 'src/app/shared/interfaces/user.interface';

@Component({
  selector: 'app-lobby-page',
  templateUrl: './lobby-page.component.html',
  styleUrls: ['./lobby-page.component.css']
})
export class LobbyPageComponent {
  lobbyParticipants: string[] = [];
  currentLobbyId: number = 0;
  currentLobbyParticipant!: User;

  constructor(public lobbyService: LobbyService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    // this.signalRService.startConnection();
    // this.signalRService.addListener();   
    // this.startHttpRequest();
    this.route.params.subscribe(params => {
      this.currentLobbyId = params['lobbyId'];
    });
    this.lobbyParticipants = this.lobbyService.lobbyParticipants;
    this.currentLobbyParticipant = this.lobbyService.currentLobbyParticipant;
    //this.currentLobbyId = this.lobbyService.lobbyId;
  }

  getProfileInfo(userIndex: number) {
    return this.lobbyParticipants[userIndex] == this.lobbyService.currentLobbyParticipant.username ? 'your-user-profile' : 'user-profile';
  }

  startGame(): void {
    this.router.navigate(['/game', this.currentLobbyId]);
  }

  leaveGame(): void {
    this.router.navigate(['']);
  }

  readyPlayer(): void {
    console.log("Player " + this.currentLobbyParticipant.username + " is ready");
  }
}
