import {Injectable} from '@angular/core';
import {BehaviorSubject, map} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LoadingIndicatorService {
  private _counter$ = new BehaviorSubject<number>(0);
  public readonly loading$ = this._counter$.pipe(
    map((value) => {
      return value > 0;
    })
  );
  
  public increment(): void {
    this._counter$.next(this._counter$.getValue() + 1);
  }

  public decrement(): void {
    this._counter$.next(this._counter$.getValue() - 1);
  }
}
