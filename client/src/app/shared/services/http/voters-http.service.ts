import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiPaths } from '../../../core/constants/api-paths';
import { ICreateVoter, IUpdateVoter, IVote, IVoter } from '../../models/voter';
import { IApiResult } from '../../../core/models/api-result';

@Injectable({
  providedIn: 'root',
})
export class VotersHttpService {
  private baseUrl = environment.apiUrl;

  private httpClient = inject(HttpClient);

  public get(id: number): Observable<IVoter> {
    return this.httpClient.get<IVoter>(
      `${this.baseUrl}/${ApiPaths.Voters}/${id}`,
    );
  }

  public getAll(): Observable<IVoter[]> {
    return this.httpClient.get<IVoter[]>(`${this.baseUrl}/${ApiPaths.Voters}`);
  }

  public create(createVoter: ICreateVoter): Observable<IApiResult> {
    return this.httpClient.post<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Voters}`,
      createVoter,
    );
  }

  public update(id: number, updateVoter: IUpdateVoter): Observable<IApiResult> {
    return this.httpClient.put<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Voters}/${id}`,
      updateVoter,
    );
  }

  public vote(vote: IVote): Observable<IApiResult> {
    return this.httpClient.put<IApiResult>(
      `${this.baseUrl}/${ApiPaths.VotersVote}`,
      vote,
    );
  }

  public delete(id: number): Observable<IApiResult> {
    return this.httpClient.delete<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Voters}/${id}`,
    );
  }
}
