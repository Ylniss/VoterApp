import { inject, Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, tap } from 'rxjs';
import { IApiResult } from '../../core/models/api-result';
import { CandidatesHttpService } from '../../shared/services/http/candidates-http.service';
import {
  ICreateCandidate,
  IUpdateCandidate,
} from '../../shared/models/candidate';

@Injectable({
  providedIn: 'root',
})
export class CandidateService {
  candidatesHttpService = inject(CandidatesHttpService);
  protected toastr = inject(ToastrService);

  public create(createCandidate: ICreateCandidate): Observable<IApiResult> {
    return this.candidatesHttpService
      .create(createCandidate)
      .pipe(tap((result) => this.toastr.success(result.message)));
  }

  public update(
    id: number,
    updateCandidate: IUpdateCandidate,
  ): Observable<IApiResult> {
    return this.candidatesHttpService
      .update(id, updateCandidate)
      .pipe(tap((result) => this.toastr.success(result.message)));
  }

  public delete(id: number): Observable<IApiResult> {
    return this.candidatesHttpService
      .delete(id)
      .pipe(tap((result) => this.toastr.success(result.message)));
  }
}
