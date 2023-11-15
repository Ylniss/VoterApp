import { Component, DestroyRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-base',
  standalone: true,
  imports: [CommonModule],
  template: '<div></div>',
})
export class BaseComponent {
  protected destroyedRef = inject(DestroyRef);

  protected bind<T>(source: Observable<T>, target: Subject<T>): void {
    source
      .pipe(takeUntilDestroyed(this.destroyedRef))
      .subscribe((data) => target.next(data));
  }
}
