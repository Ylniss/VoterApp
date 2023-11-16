import { Component, DestroyRef, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup } from '@angular/forms';
import { catchError, Observable, share, Subject, tap, throwError } from 'rxjs';
import { LoadingIndicatorService } from '../../services/loading-indicator.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IApiError } from '../../../core/models/api-error';
import { FormsService } from '../../services/forms.service';
import { Messages } from '../../../core/constants/messages';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-base-form',
  standalone: true,
  template: ``,
  imports: [CommonModule],
})
export class BaseFormComponent<T> implements OnInit {
  @Input()
  public submitButtonText: string = 'Save';

  public form!: FormGroup;
  public submit$ = new Subject<T>();
  public cancel$ = new Subject<void>();

  protected destroyedRef = inject(DestroyRef);

  protected readonly Messages = Messages;

  protected toastr = inject(ToastrService);
  protected formsService = inject(FormsService);
  protected loadingIndicatorService = inject(LoadingIndicatorService);

  public loading$ = this.loadingIndicatorService.loading$.pipe(share());

  protected _initEntity!: T;

  public get initEntity(): T {
    return this._initEntity;
  }

  @Input()
  public set initEntity(initEntity: T) {
    this._initEntity = initEntity;
    if (initEntity) this.populateForm(initEntity);
  }

  public ngOnInit(): void {
    this.loading$.subscribe((loading) => {
      loading ? this.form?.disable() : this.form?.enable();
    });
  }

  public submit(): void {
    this.submit$.next(this.form.value);
    this.form.reset();
  }

  protected onSubmit(): Observable<T> {
    return this.submit$.pipe(
      takeUntilDestroyed(this.destroyedRef),
      catchError((error: IApiError) => {
        this.formsService.handleRemoteErrors(error, this.form);
        return throwError(() => error);
      }),
    );
  }

  protected onCancel(): Observable<void> {
    return this.cancel$.pipe(
      takeUntilDestroyed(this.destroyedRef),
      tap(() => {
        this.submit$.unsubscribe();
      }),
    );
  }

  protected populateForm(_: T): void {
    /*
     *  Override in derived class like this example:
     *  this.formsService.populateControl(this.control1, myEntity.field1);
     *  this.formsService.populateControl(this.control2, myEntity.field2);
     *  etc...
     * */
  }
}
