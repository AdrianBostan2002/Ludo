import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './webpages/home-page/home-page.component';
import { LobbyPageComponent } from './webpages/lobby-page/lobby-page.component';
import { GamePageComponent } from './webpages/game-page/game-page.component';

const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'lobby/:lobbyId', component: LobbyPageComponent },
  { path: 'game/:gameId', component: GamePageComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
