import {Component, Input} from '@angular/core';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-logo',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="logo" [class.logo-small]="small" [class.logo-large]="!small">{{title}}</div>
  `,
  styleUrls: ['./logo.component.scss']
})
export class LogoComponent {
  title = 'Voter App';

  @Input() small = false;
}
