import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Piece } from '../shared/entities/piece';

@Component({
  selector: 'app-winning-modal',
  templateUrl: './winning-modal.component.html',
  styleUrls: ['./winning-modal.component.css']
})
export class WinningModalComponent {
  //@Input() name!: string;
  players!: string[];

  constructor(private router: Router) { }

  homePageRedirect() {
    this.router.navigate(['']);
  }
}
