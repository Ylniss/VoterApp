import { inject, Injectable } from '@angular/core';
import { ElectionsHttpService } from '../../shared/services/http/elections-http.service';
import {
  ICreateElection,
  ICreateElectionResult,
  IElection,
  IElectionPublic,
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
import { UUID } from 'crypto';

@Injectable({
  providedIn: 'root',
})
export class ElectionsService {
  public createElectionSuccess$ = new Subject<ICreateElectionResult>();
  public createElectionError$ = new Subject<IApiError>();
  private electionsHttpService = inject(ElectionsHttpService);
  private _election$ = new ReplaySubject<IElection>();
  public election$: Observable<IElection> = this._election$.asObservable();
  private _electionPublic$ = new ReplaySubject<IElectionPublic>();
  public electionPublic$: Observable<IElectionPublic> =
    this._electionPublic$.asObservable();

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

  public loadByRoomCode(roomCode: UUID): void {
    this.electionsHttpService
      .getByRoomCode(roomCode)
      .pipe(tap((electionPublic) => this._electionPublic$.next(electionPublic)))
      .subscribe();
  }
}
