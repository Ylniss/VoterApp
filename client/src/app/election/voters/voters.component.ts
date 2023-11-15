import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateVoterComponent } from './create-voter/create-voter.component';
import { VotersService } from '../services/voters.service';

@Component({
  selector: 'app-voters',
  standalone: true,
  imports: [CommonModule, CreateVoterComponent],
  templateUrl: './voters.component.html',
})
export class VotersComponent {
  votersService = inject(VotersService);
}
