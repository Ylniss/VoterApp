import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export enum ReadOrUpdateMode {
  READ = 1,
  UPDATE,
}

@Injectable({
  providedIn: 'root',
})
export abstract class BaseComponentModesService<T> {
  public defaultMode!: T;
  private _modes$ = new BehaviorSubject<Map<number, T>>(new Map<number, T>());
  public modes$ = this._modes$.asObservable();

  public setMode(itemId: number, mode: T): void {
    this._modes$.next(this._modes$.value.set(itemId, mode));
  }

  public setModeAndResetRemaining(itemId: number, mode: T): void {
    this.resetAllToDefaultMode();
    this.setMode(itemId, mode);
  }

  public resetAllToDefaultMode(): void {
    if (!this.defaultMode)
      throw new Error(
        'BaseComponentModesService default mode not initialized.',
      );

    const currentModes = this._modes$.value;
    currentModes.forEach((_, key) => currentModes.set(key, this.defaultMode));
    this._modes$.next(currentModes);
  }
}
