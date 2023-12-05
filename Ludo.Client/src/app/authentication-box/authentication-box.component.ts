import { Component } from '@angular/core';
import { LobbyService } from 'src/app/services/lobby.service';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { RoleType } from '../shared/enums/roletype.enum';


@Component({
  selector: 'app-authentication-box',
  templateUrl: './authentication-box.component.html',
  styleUrls: ['./authentication-box.component.css']
})
export class AuthenticationBoxComponent {
  constructor(public lobbyService: LobbyService, private http: HttpClient, private router: Router) { }

  name = new FormControl('');
  lobbyId = new FormControl();
  currentLobbyId: number = 0;

  ngOnInit() {
    // this.signalRService.startConnection();
    // this.signalRService.addListener();   
    // this.startHttpRequest();
    //this.currentLobbyId = this.lobbyService.lobbyId;
    this.lobbyService.lobbyId$.subscribe({
      next: (data: number) => this.currentLobbyId = data
    });
  }


  createNewLobby() {
    if (this.name.value) {
      this.lobbyService.createLobbyConnection(this.name.value);
      this.lobbyService.addLobbyListener();
      this.startHttpRequest();
      this.lobbyService.createLobbyParticipant(this.name.value, RoleType.Owner);
    }
  }

  joinLobby() {
    if (this.name.value && this.lobbyId.value != 0) {
      this.lobbyService.joinLobbyConnection(this.lobbyId.value as number, this.name.value as string);
      this.lobbyService.addLobbyListener();
      this.lobbyService.createLobbyParticipant(this.name.value, RoleType.Regular);
    }
  }

  private startHttpRequest = () => {
    this.http.get('https://localhost:7192/api/Lobby')
      .subscribe(res => {
        console.log(res);
      })
  }
}
