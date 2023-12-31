import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { HomePageComponent } from './webpages/home-page/home-page.component';
import { LobbyPageComponent } from './webpages/lobby-page/lobby-page.component';
import { AuthenticationBoxComponent } from './authentication-box/authentication-box.component';
import { GamePageComponent } from './webpages/game-page/game-page.component';
import { CellComponent } from './game-components/cell/cell.component';
import { PieceComponent } from './game-components/piece/piece.component';
import { HomeComponent } from './game-components/home/home.component';
import { DiceRollComponent } from './game-components/dice-roll/dice-roll.component'; 
import { BoardComponent } from './board/board.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { WinningModalComponent } from './winning-modal/winning-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    LobbyPageComponent,
    AuthenticationBoxComponent,
    GamePageComponent,
    CellComponent,
    PieceComponent,
    HomeComponent,
    DiceRollComponent,
    BoardComponent,
    WinningModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
