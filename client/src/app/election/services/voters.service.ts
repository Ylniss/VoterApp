import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Subject, switchMap, tap } from 'rxjs';
import { ICreateVoter, IVoter } from '../../shared/models/voter';
import { VotersHttpService } from '../../shared/services/http/voters-http.service';
import { IApiError } from '../../core/models/api-error';

@Injectable({
  providedIn: 'root',
})
export class VotersService {
  votersHttpService = inject(VotersHttpService);

  public createVoterSuccess$ = new Subject<IVoter>();
  public createVoterError$ = new Subject<IApiError>();

  public updateVoterSuccess$ = new Subject<IVoter>();
  public updateVoterError$ = new Subject<IApiError>();

  private _voters = new BehaviorSubject<IVoter[]>([]);
  public voters$ = this._voters.asObservable();

  public create(createVoter: ICreateVoter): void {
    this.votersHttpService
      .create(createVoter)
      .pipe(
        switchMap((apiResult) => this.votersHttpService.get(apiResult.id)),
        tap((voter) => this.addVoterToList(voter)),
        catchError((error: any) => {
          this.createVoterError$.next(error.error);
          throw error;
        }),
      )
      .subscribe((voter) => this.createVoterSuccess$.next(voter));
  }

  private addVoterToList(voter: IVoter): void {
    this._voters.next([...this._voters.value, voter]);
  }
}
