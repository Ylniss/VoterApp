import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IVoterPublic } from '../../../shared/models/voter';
import { ElectionsService } from '../../services/elections.service';

@Component({
  selector: 'app-read-voter-public',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-voter-public.component.html',
})
export class ReadVoterPublicComponent {
  @Input() voter!: IVoterPublic;

  private electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.voter)
      throw new Error('ReadVoterCreatorComponent voter is not initialized.');
  }
}
