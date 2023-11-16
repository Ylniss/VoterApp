import { inject, Injectable } from '@angular/core';
import { ElectionsHttpService } from '../../shared/services/http/elections-http.service';
import {
  ICreateElection,
  ICreateElectionResult,
  IElection,
  IElectionPublic,
} from '../../shared/models/election';
import { map, mergeMap, Observable, ReplaySubject, tap } from 'rxjs';
import { IApiResult } from '../../core/models/api-result';
import { UUID } from 'crypto';

@Injectable({
  providedIn: 'root',
})
export class ElectionsService {
  private electionsHttpService = inject(ElectionsHttpService);
  private _election$ = new ReplaySubject<IElection>();
  public election$: Observable<IElection> = this._election$.asObservable();
  private _electionPublic$ = new ReplaySubject<IElectionPublic>();
  public electionPublic$: Observable<IElectionPublic> =
    this._electionPublic$.asObservable();

  public create(
    createElection: ICreateElection,
  ): Observable<ICreateElectionResult> {
    return this.electionsHttpService.create(createElection).pipe(
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
    );
  }

  public loadByElectionId(electionId: number): Observable<IElection> {
    return this.electionsHttpService
      .get(electionId)
      .pipe(tap((election) => this._election$.next(election)));
  }

  public loadByRoomCode(roomCode: UUID): Observable<IElectionPublic> {
    return this.electionsHttpService
      .getByRoomCode(roomCode)
      .pipe(
        tap((electionPublic) => this._electionPublic$.next(electionPublic)),
      );
  }
}
