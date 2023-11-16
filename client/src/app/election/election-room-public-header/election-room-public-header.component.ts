import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IElectionPublic } from '../../shared/models/election';
import { RoomCodeDisplayComponent } from '../../shared/components/room-code-display/room-code-display.component';

@Component({
  selector: 'app-election-room-public-header',
  standalone: true,
  imports: [CommonModule, RoomCodeDisplayComponent],
  templateUrl: './election-room-public-header.component.html',
})
export class ElectionRoomPublicHeaderComponent implements OnInit {
  @Input() election!: IElectionPublic;

  ngOnInit(): void {
    if (!this.election) throw new Error('Election not initialized');
  }
}
