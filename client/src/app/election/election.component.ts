import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateElectionComponent } from '../home/create-election/create-election.component';
import { JoinElectionComponent } from '../home/join-election/join-election.component';
import { LogoComponent } from '../shared/components/logo/logo.component';
import { VoteComponent } from './vote/vote.component';
import { VotersComponent } from './voters/voters.component';
import { CandidatesComponent } from './candidates/candidates.component';
import { ActivatedRoute, Router } from '@angular/router';
import { ElectionsService } from './services/elections.service';

@Component({
  selector: 'app-election',
  standalone: true,
  imports: [
    CommonModule,
    CreateElectionComponent,
    JoinElectionComponent,
    LogoComponent,
    VoteComponent,
    VotersComponent,
    CandidatesComponent,
  ],
  templateUrl: './election.component.html',
  styleUrl: './election.component.scss',
})
export class ElectionComponent {
  route = inject(ActivatedRoute);
  router = inject(Router);
  electionService = inject(ElectionsService);
}
