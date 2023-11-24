import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LobbyService } from 'src/app/services/lobby.service';

@Component({
  selector: 'app-lobby-page',
  templateUrl: './lobby-page.component.html',
  styleUrls: ['./lobby-page.component.css']
})
export class LobbyPageComponent {
  lobbyParticipants: string[] = [];
  currentLobbyId: number = 0;

  constructor(public lobbyService: LobbyService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    // this.signalRService.startConnection();
    // this.signalRService.addListener();   
    // this.startHttpRequest();
    // this.route.params.subscribe(params => {
    //   this.currentLobbyId = params['lobbyId'];
    // });
    this.lobbyParticipants = this.lobbyService.lobbyParticipants;
    this.currentLobbyId = this.lobbyService.lobbyId;
  }
}
