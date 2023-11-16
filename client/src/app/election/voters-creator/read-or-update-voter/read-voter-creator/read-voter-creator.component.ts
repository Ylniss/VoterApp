import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IVoter } from '../../../../shared/models/voter';
import { VotersCreatorComponentModesService } from '../../../services/voters-creator-component-modes.service';
import { ReadOrUpdateMode } from '../../../../shared/services/base-component-modes.service';
import { VotersService } from '../../../services/voters.service';
import { ElectionsService } from '../../../services/elections.service';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-read-voter-creator',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-voter-creator.component.html',
})
export class ReadVoterCreatorComponent implements OnInit {
  @Input() voter!: IVoter;

  private votersCreatorComponentModes = inject(
    VotersCreatorComponentModesService,
  );
  private votersService = inject(VotersService);
  private electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.voter)
      throw new Error('ReadVoterCreatorComponent voter is not initialized.');
  }

  public openRollHistoryEdit(): void {
    this.votersCreatorComponentModes.setModeAndResetRemaining(
      this.voter.id,
      ReadOrUpdateMode.UPDATE,
    );
  }

  public deleteVoter(): void {
    this.votersService
      .delete(this.voter.id)
      .pipe(
        switchMap((result) =>
          this.electionService.loadByElectionId(this.voter.electionId),
        ),
      )
      .subscribe();
  }
}
