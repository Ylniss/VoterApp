import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IVoter } from '../../../shared/models/voter';

@Component({
  selector: 'app-read-or-update-voter',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-or-update-voter.component.html',
  styleUrl: './read-or-update-voter.component.scss',
})
export class ReadOrUpdateVoterComponent implements OnInit {
  @Input() voter!: IVoter;

  ngOnInit(): void {
    if (!this.voter)
      throw new Error('ReadOrUpdateVoterComponent voter is not initialized.');
  }
}
