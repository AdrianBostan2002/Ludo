import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  public data: string[]=[];

  // private hubConnection: signalR.HubConnection;
  
  //   public startConnection = () => {
  //     this.hubConnection = new signalR.HubConnectionBuilder()
  //                             .withUrl('https://localhost:7192/testHub')
  //                             .build();
  //     this.hubConnection
  //       .start()
  //       .then(() => console.log('Connection started'))
  //       .catch(err => console.log('Error while starting connection: ' + err))
  //   }
    
  //   public addListener = () => {
  //     this.hubConnection.on('newMessageToOtherClients', (data) => {
  //       this.data = data;
  //       console.log(data);
  //     });
  //   }
}
