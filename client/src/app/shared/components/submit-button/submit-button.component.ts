import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingIndicatorService } from '../../services/loading-indicator.service';
import { LoadingIndicatorComponent } from '../loading-indicator/loading-indicator.component';

@Component({
  selector: 'app-submit-button',
  standalone: true,
  imports: [CommonModule, LoadingIndicatorComponent],
  templateUrl: './submit-button.component.html',
})
export class SubmitButtonComponent implements OnInit {
  @Input() disabled = false;
  @Input() text: string = 'Save';
  @Input() loadingText: string = 'Loading...';

  loadingIndicatorService = inject(LoadingIndicatorService);

  public loading = false;

  public ngOnInit(): void {
    this.loadingIndicatorService.loading$.subscribe((loading) => {
      loading ? (this.loading = true) : (this.loading = false);
    });
  }
}
