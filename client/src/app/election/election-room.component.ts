import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateElectionComponent } from '../home/create-election/create-election.component';
import { JoinElectionComponent } from '../home/join-election/join-election.component';
import { LogoComponent } from '../shared/components/logo/logo.component';
import { VoteComponent } from './vote/vote.component';
import { VotersCreatorComponent } from './voters-creator/voters-creator.component';
import { CandidatesCreatorComponent } from './candidates-creator/candidates-creator.component';
import { ElectionsService } from './services/elections.service';
import { RoomCodeUrlGrabberService } from './services/room-code-url-grabber.service';
import { UUID } from 'crypto';
import { switchMap } from 'rxjs';
import { ElectionRoomCreatorHeaderComponent } from './election-room-creator-header/election-room-creator-header.component';
import { ElectionRoomPublicHeaderComponent } from './election-room-public-header/election-room-public-header.component';

@Component({
  selector: 'app-election',
  standalone: true,
  imports: [
    CommonModule,
    CreateElectionComponent,
    JoinElectionComponent,
    LogoComponent,
    VoteComponent,
    VotersCreatorComponent,
    CandidatesCreatorComponent,
    ElectionRoomCreatorHeaderComponent,
    ElectionRoomPublicHeaderComponent,
  ],
  templateUrl: './election-room.component.html',
})
export class ElectionRoomComponent implements OnInit {
  public electionService = inject(ElectionsService);
  public roomCode!: UUID;
  private roomCodeUrlGrabber = inject(RoomCodeUrlGrabberService);
  private electionId?: number;

  ngOnInit(): void {
    this.loadElectionByRoomCodeFromUrl();
    if (!this.roomCode) return; // ssr is not able to get roomcode from url, exit to prevent errors

    this.electionId = Number(localStorage.getItem(this.roomCode));

    if (this.electionId) {
      this.electionService
        .isRoomCodeAndElectionIdPairValid(this.roomCode, this.electionId)
        .pipe(
          switchMap((isValid) => {
            if (isValid)
              // has 'admin' rights, load from non-public endpoint
              return this.electionService.loadByElectionId(this.electionId!);
            else return this.electionService.loadByRoomCode(this.roomCode); // load from public endpoint
          }),
        )
        .subscribe();
    } else {
      console.log('loading electionPublic by room code');
      this.electionService.loadByRoomCode(this.roomCode).subscribe();
    }
  }

  private loadElectionByRoomCodeFromUrl(): void {
    const maybeRoomCode = this.roomCodeUrlGrabber.getRoomCode();

    if (maybeRoomCode) {
      this.roomCode = maybeRoomCode;
      console.log(`oninit - ElectionComponent, roomcode: ${this.roomCode}`);
    }
  }
}
