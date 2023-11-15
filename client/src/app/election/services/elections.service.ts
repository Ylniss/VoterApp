import { inject, Injectable } from '@angular/core';
import { ElectionsHttpService } from '../../shared/services/http/elections-http.service';
import {
  ICreateElection,
  ICreateElectionResult,
  IElection,
} from '../../shared/models/election';
import {
  catchError,
  map,
  mergeMap,
  Observable,
  ReplaySubject,
  Subject,
  tap,
} from 'rxjs';
import { IApiError } from '../../core/models/api-error';
import { IApiResult } from '../../core/models/api-result';

@Injectable({
  providedIn: 'root',
})
export class ElectionsService {
  electionsHttpService = inject(ElectionsHttpService);
  public createElectionSuccess$ = new Subject<ICreateElectionResult>();
  public createElectionError$ = new Subject<IApiError>();
  private _election$ = new ReplaySubject<IElection>();
  public election$: Observable<IElection> = this._election$.asObservable();

  public create(createElection: ICreateElection): void {
    this.electionsHttpService
      .create(createElection)
      .pipe(
        mergeMap((apiResult: IApiResult) =>
          this.electionsHttpService.get(apiResult.id).pipe(
            map((election: IElection) => {
              const createElectionResult: ICreateElectionResult = {
                apiResult: apiResult,
                election: election,
              };

              return createElectionResult;
            }),
          ),
        ),
        tap((createElectionResult) => {
          this._election$.next(createElectionResult.election);
        }),
        catchError((error: any) => {
          this.createElectionError$.next(error.error);
          throw error;
        }),
      )
      .subscribe((createElectionResult) => {
        this.createElectionSuccess$.next(createElectionResult);
      });
  }
}
