import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TestService } from './services/test.service';
import { FormControl, FormGroup } from '@angular/forms';
import { LobbyService } from './services/lobby.service';
import { GameService } from './services/game.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public lobbyService: LobbyService, private http: HttpClient, private gameService: GameService) { }

  name = new FormControl('');
  lobbyId = new FormControl(0);
  lobbyParticipants: string[] = [];
  currentLobbyId: number = 0;

  ngOnInit() {
    // this.signalRService.startConnection();
    // this.signalRService.addListener();   
    // this.startHttpRequest();
    this.lobbyParticipants = this.lobbyService.lobbyParticipants;
    this.currentLobbyId = this.lobbyService.lobbyId;
  }


  createNewLobby() {
    if (this.name.value) {
      this.lobbyService.createLobbyConnection(this.name.value);
      this.lobbyService.addLobbyListener();
      this.startHttpRequest();
    }
  }

  joinLobby() {
    if (this.name.value != '' && this.lobbyId.value != 0) {
      this.lobbyService.joinLobbyConnection(this.lobbyId.value as number, this.name.value as string);
      this.lobbyService.addLobbyListener();
      //this.startHttpRequest();
    }
  }

  startGame(){
    this.lobbyService.DisconnectFromHub();
    this.gameService.startGame(this.lobbyId.value as number);
    this.gameService.addGameListener();
  }

  private startHttpRequest = () => {
    // this.http.get('https://localhost:7192/api/Lobby')
    //   .subscribe(res => {
    //     console.log(res);
    //   })
  }
}
