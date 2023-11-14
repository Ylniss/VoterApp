import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LogoComponent} from "../shared/components/logo/logo.component";
import {JoinElectionComponent} from "./join-election/join-election.component";
import {CreateElectionComponent} from "./create-election/create-election.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, LogoComponent, JoinElectionComponent, CreateElectionComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
}
