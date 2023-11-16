import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateVoterComponent } from './create-voter/create-voter.component';
import { UUID } from 'crypto';
import { ReadOrUpdateVoterComponent } from './read-or-update-voter/read-or-update-voter.component';
import { ElectionsService } from '../services/elections.service';

@Component({
  selector: 'app-voters-creator',
  standalone: true,
  imports: [CommonModule, CreateVoterComponent, ReadOrUpdateVoterComponent],
  templateUrl: './voters-creator.component.html',
})
export class VotersCreatorComponent implements OnInit {
  @Input() roomCode!: UUID;

  public electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.roomCode)
      throw new Error('VotersCreatorComponent roomCode not initialized');
  }
}
