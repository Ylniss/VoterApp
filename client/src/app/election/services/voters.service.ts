import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ICreateVoter, IUpdateVoter, IVote } from '../../shared/models/voter';
import { VotersHttpService } from '../../shared/services/http/voters-http.service';
import { IApiResult } from '../../core/models/api-result';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class VotersService {
  votersHttpService = inject(VotersHttpService);
  protected toastr = inject(ToastrService);

  public create(createVoter: ICreateVoter): Observable<IApiResult> {
    return this.votersHttpService
      .create(createVoter)
      .pipe(tap((result) => this.toastr.success(result.message)));
  }

  public update(id: number, updateVoter: IUpdateVoter): Observable<IApiResult> {
    return this.votersHttpService
      .update(id, updateVoter)
      .pipe(tap((result) => this.toastr.success(result.message)));
  }

  public delete(id: number): Observable<IApiResult> {
    return this.votersHttpService
      .delete(id)
      .pipe(tap((result) => this.toastr.success(result.message)));
  }

  public vote(vote: IVote): Observable<IApiResult> {
    return this.votersHttpService
      .vote(vote)
      .pipe(tap(() => this.toastr.success('Vote successful.')));
  }
}
