import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IElection } from '../../shared/models/election';
import { RoomCodeDisplayComponent } from '../../shared/components/room-code-display/room-code-display.component';

@Component({
  selector: 'app-election-room-creator-header',
  standalone: true,
  imports: [CommonModule, RoomCodeDisplayComponent],
  templateUrl: './election-room-creator-header.component.html',
})
export class ElectionRoomCreatorHeaderComponent implements OnInit {
  @Input() election!: IElection;

  ngOnInit(): void {
    if (!this.election)
      throw new Error('ElectionRoomCreator election not initialized');
  }
}
