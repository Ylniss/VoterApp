import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UUID } from 'crypto';
import { ElectionsService } from '../services/elections.service';

@Component({
  selector: 'app-candidates-public',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './candidates-public.component.html',
})
export class CandidatesPublicComponent {
  @Input() roomCode!: UUID;

  public electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.roomCode)
      throw new Error('CandidatesPublicComponent roomCode not initialized');
  }
}
