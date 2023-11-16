import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UUID } from 'crypto';
import { ElectionsService } from '../services/elections.service';
import { CreateVoterComponent } from '../voters-creator/create-voter/create-voter.component';
import { ReadOrUpdateVoterComponent } from '../voters-creator/read-or-update-voter/read-or-update-voter.component';
import { ReadOrUpdateCandidateComponent } from './read-or-update-candidate/read-or-update-candidate.component';
import { CreateCandidateComponent } from './create-candidate/create-candidate.component';

@Component({
  selector: 'app-candidates-creator',
  standalone: true,
  imports: [
    CommonModule,
    CreateVoterComponent,
    ReadOrUpdateVoterComponent,
    ReadOrUpdateCandidateComponent,
    CreateCandidateComponent,
  ],
  templateUrl: './candidates-creator.component.html',
})
export class CandidatesCreatorComponent {
  @Input() roomCode!: UUID;

  public electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.roomCode)
      throw new Error('CandidatesCreatorComponent roomCode not initialized');
  }
}
