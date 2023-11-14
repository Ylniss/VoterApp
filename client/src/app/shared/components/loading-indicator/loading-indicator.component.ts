import {Component, Input} from '@angular/core';
import {CommonModule} from '@angular/common';
import {NgxSpinnerModule} from "ngx-spinner";

@Component({
  selector: 'app-loading-indicator',
  standalone: true,
  imports: [CommonModule, NgxSpinnerModule],
  templateUrl: './loading-indicator.component.html',
  styleUrl: './loading-indicator.component.scss'
})
export class LoadingIndicatorComponent {
  @Input()
  public text: string = "Loading...";
}
