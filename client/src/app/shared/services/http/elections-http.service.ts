import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { ICreateElection, ICreateElectionResult } from '../../models/election';
import { ApiPaths } from '../../../core/constants/api-paths';

@Injectable({
  providedIn: 'root',
})
export class ElectionsHttpService {
  baseUrl = environment.apiUrl;

  httpClient = inject(HttpClient);

  create(createElection: ICreateElection): Observable<ICreateElectionResult> {
    return this.httpClient.post<ICreateElectionResult>(
      `${this.baseUrl}/${ApiPaths.Elections}`,
      createElection,
    );
  }
}
