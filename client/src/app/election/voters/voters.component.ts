import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateVoterComponent } from './create-voter/create-voter.component';
import { UUID } from 'crypto';

@Component({
  selector: 'app-voters',
  standalone: true,
  imports: [CommonModule, CreateVoterComponent],
  templateUrl: './voters.component.html',
})
export class VotersComponent implements OnInit {
  @Input() roomCode!: UUID;

  ngOnInit(): void {
    console.log(`oninit - VotersComponent room code: ${this.roomCode}`);
  }
}
