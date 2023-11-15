import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiPaths } from '../../../core/constants/api-paths';
import { IApiResult } from '../../../core/models/api-result';
import {
  ICandidate,
  ICreateCandidate,
  IUpdateCandidate,
} from '../../models/candidate';

@Injectable({
  providedIn: 'root',
})
export class CandidatesHttpService {
  private baseUrl = environment.apiUrl;

  private httpClient = inject(HttpClient);

  public get(id: number): Observable<ICandidate> {
    return this.httpClient.get<ICandidate>(
      `${this.baseUrl}/${ApiPaths.Candidates}/${id}`,
    );
  }

  public getAll(): Observable<ICandidate[]> {
    return this.httpClient.get<ICandidate[]>(
      `${this.baseUrl}/${ApiPaths.Candidates}`,
    );
  }

  public create(createCandidate: ICreateCandidate): Observable<IApiResult> {
    return this.httpClient.post<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Candidates}`,
      createCandidate,
    );
  }

  public update(
    id: number,
    updateCandidate: IUpdateCandidate,
  ): Observable<IApiResult> {
    return this.httpClient.put<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Candidates}/${id}`,
      updateCandidate,
    );
  }

  public delete(id: number): Observable<IApiResult> {
    return this.httpClient.delete<IApiResult>(
      `${this.baseUrl}/${ApiPaths.Candidates}/${id}`,
    );
  }
}
