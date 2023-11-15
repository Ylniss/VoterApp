import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class LoadingIndicatorService {
  spinner = inject(NgxSpinnerService);

  private _counter$ = new BehaviorSubject<number>(0);
  public readonly loading$ = this._counter$.pipe(
    map((value) => {
      if (value <= 0) {
        this.spinner.show();
        return false;
      } else {
        this.spinner.hide();
        return true;
      }
    }),
  );

  public increment(): void {
    this._counter$.next(this._counter$.getValue() + 1);
  }

  public decrement(): void {
    this._counter$.next(this._counter$.getValue() - 1);
  }
}
