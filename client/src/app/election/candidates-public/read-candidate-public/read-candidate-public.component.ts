import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ElectionsService } from '../../services/elections.service';
import { ICandidatePublic } from '../../../shared/models/candidate';

@Component({
  selector: 'app-read-candidate-public',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-candidate-public.component.html',
})
export class ReadCandidatePublicComponent {
  @Input() candidate!: ICandidatePublic;

  private electionService = inject(ElectionsService);

  ngOnInit(): void {
    if (!this.candidate)
      throw new Error('ReadCandidatePublicComponent voter is not initialized.');
  }
}
