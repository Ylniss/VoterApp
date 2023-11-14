import {Component, inject} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateElectionService} from "./create-election.service";

@Component({
  selector: 'app-create-election',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './create-election.component.html'
})
export class CreateElectionComponent {
  createElectionService = inject(CreateElectionService);


}
