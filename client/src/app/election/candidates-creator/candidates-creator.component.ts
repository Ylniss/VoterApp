import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UUID } from 'crypto';
import { ElectionsService } from '../services/elections.service';

@Component({
  selector: 'app-candidates-creator',
  standalone: true,
  imports: [CommonModule],
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
