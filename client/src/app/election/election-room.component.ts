import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateElectionComponent } from '../home/create-election/create-election.component';
import { JoinElectionComponent } from '../home/join-election/join-election.component';
import { LogoComponent } from '../shared/components/logo/logo.component';
import { VoteComponent } from './vote/vote.component';
import { VotersComponent } from './voters/voters.component';
import { CandidatesComponent } from './candidates/candidates.component';
import { ElectionsService } from './services/elections.service';
import { RoomCodeUrlGrabberService } from './services/room-code-url-grabber.service';
import { UUID } from 'crypto';

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
  templateUrl: './election-room.component.html',
  styleUrl: './election-room.component.scss',
})
export class ElectionRoomComponent implements OnInit {
  public electionService = inject(ElectionsService);
  public roomCode!: UUID;
  private roomCodeUrlGrabber = inject(RoomCodeUrlGrabberService);

  ngOnInit(): void {
    this.loadElectionByRoomCodeFromUrl();
    // todo check if admin -> send req to endpoint that validates roomcode-electionid pair from local storage
    // if yes then send req to get endpoint for election$ (nonpubilc)
  }

  private loadElectionByRoomCodeFromUrl(): void {
    const maybeRoomCode = this.roomCodeUrlGrabber.getRoomCode();

    if (maybeRoomCode) {
      this.roomCode = maybeRoomCode;
      console.log(`oninit - ElectionComponent, roomcode: ${this.roomCode}`);
      console.log('loading electionPublic by room code');
      this.electionService.loadByRoomCode(this.roomCode).subscribe();
    }
  }
}
