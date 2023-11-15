import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import {
  ICreateElection,
  IElection,
  IElectionPublic,
} from '../../models/election';
import { ApiPaths } from '../../../core/constants/api-paths';
import { IApiResult } from '../../../core/models/api-result';
import { UUID } from 'crypto';

@Injectable({
  providedIn: 'root',
})
export class ElectionsHttpService {
  private baseUrl = environment.apiUrl;

  private httpClient = inject(HttpClient);

  public get(id: number): Observable<IElection> {
    return this.httpClient.get<IElection>(
      `${this.baseUrl}/${ApiPaths.Elections}/${id}`,
    );
  }

  public getByRoomCode(roomCode: UUID): Observable<IElectionPublic> {
    return this.httpClient.get<IElectionPublic>(
      `${this.baseUrl}/${ApiPaths.ElectionsRoomCode}/${roomCode}`,
    );
  }

  public create(createElection: ICreateElection): Observable<IApiResult> {
    return this.httpClient.post<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Elections}`,
      createElection,
    );
  }
}
