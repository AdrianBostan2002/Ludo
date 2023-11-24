import { Component } from '@angular/core';
import { LobbyService } from 'src/app/services/lobby.service';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-authentication-box',
  templateUrl: './authentication-box.component.html',
  styleUrls: ['./authentication-box.component.css']
})
export class AuthenticationBoxComponent {
  constructor(public lobbyService: LobbyService, private http: HttpClient, private router: Router) { }

  name = new FormControl('');
  lobbyId = new FormControl();
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
      this.router.navigate(['/lobby', this.lobbyService.lobbyId]);
    }
    console.log("lobbyId" + this.lobbyService.lobbyId);
  }

  joinLobby() {
    if (this.name.value != '' && this.lobbyId.value != 0) {
      this.lobbyService.joinLobbyConnection(this.lobbyId.value as number, this.name.value as string);
      this.lobbyService.addLobbyListener();
      //this.startHttpRequest();
      this.router.navigate(['/lobby', this.lobbyService.lobbyId]);
    }
    console.log("lobbyIdJoin" + this.lobbyService.lobbyId);
  }

  private startHttpRequest = () => {
    this.http.get('https://localhost:7192/api/Lobby')
      .subscribe(res => {
        console.log(res);
      })
  }
}
