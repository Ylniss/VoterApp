import {inject, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../../environments/environment";
import {ApiPath} from "../../enums/api-path.enum";
import {ICreateElectionResponse} from "../../models/election";

@Injectable({
  providedIn: 'root'
})
export class ElectionsHttpService {
  baseUrl = environment.apiUrl;
  
  httpClient = inject(HttpClient);

  create(topic: string): Observable<ICreateElectionResponse> {
    return this.httpClient.post<ICreateElectionResponse>(`${this.baseUrl}/${ApiPath.Elections}`, topic);
  }
}
