import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ICreateVoter } from '../../shared/models/voter';
import { VotersHttpService } from '../../shared/services/http/voters-http.service';
import { IApiResult } from '../../core/models/api-result';

@Injectable({
  providedIn: 'root',
})
export class VotersService {
  votersHttpService = inject(VotersHttpService);

  public create(createVoter: ICreateVoter): Observable<IApiResult> {
    return this.votersHttpService.create(createVoter);
  }
}
