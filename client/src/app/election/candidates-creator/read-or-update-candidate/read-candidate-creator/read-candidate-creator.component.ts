import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ElectionsService } from '../../../services/elections.service';
import { ReadOrUpdateMode } from '../../../../shared/services/base-component-modes.service';
import { switchMap } from 'rxjs';
import { ICandidate } from '../../../../shared/models/candidate';
import { CandidatesCreatorComponentModesService } from '../../../services/candidates-creator-component-modes.service';
import { CandidateService } from '../../../services/candidate.service';

@Component({
  selector: 'app-read-candidate-creator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-candidate-creator.component.html',
})
export class ReadCandidateCreatorComponent implements OnInit {
  @Input() candidate!: ICandidate;

  private candidatesCreatorComponentModes = inject(
    CandidatesCreatorComponentModesService,
  );
  private candidatesService = inject(CandidateService);
  private electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.candidate)
      throw new Error(
        'ReadCandidateCreatorComponent candidate is not initialized.',
      );
  }

  public openRollHistoryEdit(): void {
    this.candidatesCreatorComponentModes.setModeAndResetRemaining(
      this.candidate.id,
      ReadOrUpdateMode.UPDATE,
    );
  }

  public deleteCandidate(): void {
    this.candidatesService
      .delete(this.candidate.id)
      .pipe(
        switchMap((result) =>
          this.electionService.loadByElectionId(this.candidate.electionId),
        ),
      )
      .subscribe();
  }
}
