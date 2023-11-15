import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseComponent } from './base.component';
import { FormGroup } from '@angular/forms';
import { share, Subject } from 'rxjs';
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
export class BaseFormComponent<T> extends BaseComponent implements OnInit {
  @Input()
  public submitButtonText: string = 'Save';

  public form!: FormGroup;
  public submit$ = new Subject<T>();
  public submitSuccess$ = new Subject<any>();
  public submitError$ = new Subject<IApiError>();
  public cancel$ = new Subject<void>();

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
  }

  protected onSubmit(action?: (entity: T) => void): void {
    this.submit$
      .pipe(takeUntilDestroyed(this.destroyedRef))
      .subscribe((entity) => {
        if (action) action(entity);
      });
  }

  protected onSubmitSuccess(action?: (result: any) => void): void {
    this.submitSuccess$
      .pipe(takeUntilDestroyed(this.destroyedRef))
      .subscribe((result) => {
        if (action) action(result);
      });
  }

  protected onSubmitError(action?: (error: IApiError) => void): void {
    this.submitError$
      .pipe(takeUntilDestroyed(this.destroyedRef))
      .subscribe((error) => {
        this.formsService.handleRemoteErrors(error, this.form);
        if (action) action(error);
      });
  }

  protected onCancel(action?: () => void): void {
    this.cancel$.pipe(takeUntilDestroyed(this.destroyedRef)).subscribe(() => {
      this.submit$.unsubscribe();
      if (action) action();
    });
  }

  protected populateForm(_: T): void {
    /*
     *  Override in derived class like this example:
     *  this.formsService.populateControl(this.control1, myEntity.field1);
     *  this.formsService.populateControl(this.control2, myEntity.field2);
     *  etc...
     * */
  }

  protected initReactiveBindings(): void {
    /*
     *  Override in derived class like this example:
     *  this.bind(this.myService.createMyEntitySuccess$, this.submitSuccess$);
     *  this.bind(this.myService.createMyEntityError$, this.submitError$);
     *
     *  this.bind(this.myService.updateMyEntitySuccess$, this.submitSuccess$);
     *  this.bind(this.myService.updateMyEntityError$, this.submitError$);
     * */
  }
}
