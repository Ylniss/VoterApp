import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingIndicatorService } from '../../services/loading-indicator.service';

@Component({
  selector: 'app-submit-button',
  standalone: true,
  imports: [CommonModule],
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
