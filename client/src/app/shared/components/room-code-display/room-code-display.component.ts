import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-room-code-display',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './room-code-display.component.html',
  styleUrl: './room-code-display.component.scss',
})
export class RoomCodeDisplayComponent implements OnInit {
  @Input() message!: string;
  @Input() roomCode!: string;

  ngOnInit(): void {
    if (!this.message)
      throw new Error('RoomCodeDisplay message not initialized');

    if (!this.roomCode)
      throw new Error('RoomCodeDisplay roomCode not initialized');
  }
}
