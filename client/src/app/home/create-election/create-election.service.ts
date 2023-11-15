import { inject, Injectable } from '@angular/core';
import { ElectionsHttpService } from '../../shared/services/http/elections-http.service';
import {
  ICreateElection,
  ICreateElectionResult,
  IElection,
} from '../../shared/models/election';
import { catchError, EMPTY, map, Subject } from 'rxjs';
import { IApiError } from '../../core/models/api-error';

@Injectable({
  providedIn: 'root',
})
export class CreateElectionService {
  electionsHttpService = inject(ElectionsHttpService);
  public createElectionSuccess$ = new Subject<ICreateElectionResult>();
  public createElectionError$ = new Subject<IApiError>();
  private _election$ = new Subject<IElection>();
  public election$ = this._election$.asObservable();

  public create(createElection: ICreateElection): void {
    this.electionsHttpService
      .create(createElection)
      .pipe(
        map((createElectionResponse: ICreateElectionResult) => {
          const election: IElection = {
            id: createElectionResponse.id,
            roomCode: createElectionResponse.roomCode,
            topic: createElection.topic,
          };
          this._election$.next(election);
          return createElectionResponse;
        }),
        catchError((error: any) => {
          // this.appUtils.handleError(
          //   error,
          //   _('Error message'),
          // );
          this.createElectionError$.next(error.error);
          return EMPTY;
        }),
      )
      .subscribe((createElectionResult) => {
        this.createElectionSuccess$.next(createElectionResult);
      });
  }
}
