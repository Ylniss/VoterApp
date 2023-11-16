import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UUID } from 'crypto';
import { ElectionsService } from '../services/elections.service';
import { ReadOrUpdateVoterComponent } from '../voters-creator/read-or-update-voter/read-or-update-voter.component';
import { ReadVoterPublicComponent } from './read-voter-public/read-voter-public.component';

@Component({
  selector: 'app-voters-public',
  standalone: true,
  imports: [CommonModule, ReadOrUpdateVoterComponent, ReadVoterPublicComponent],
  templateUrl: './voters-public.component.html',
})
export class VotersPublicComponent {
  @Input() roomCode!: UUID;

  public electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.roomCode)
      throw new Error('VotersPublicComponent roomCode not initialized');
  }
}
