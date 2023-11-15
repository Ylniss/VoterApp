import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseFormComponent } from '../../../shared/components/base/base-form.component';
import { ICreateVoter } from '../../../shared/models/voter';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Validation } from '../../../core/constants/validation';
import { VotersService } from '../../services/voters.service';
import { ValidationMessagesDirective } from '../../../core/directives/validation-messages.directive';
import { SubmitButtonComponent } from '../../../shared/components/submit-button/submit-button.component';
import { IApiResult } from '../../../core/models/api-result';

@Component({
  selector: 'app-create-voter',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ValidationMessagesDirective,
    SubmitButtonComponent,
  ],
  templateUrl: './create-voter.component.html',
  styleUrl: './create-voter.component.scss',
})
export class CreateVoterComponent
  extends BaseFormComponent<ICreateVoter>
  implements OnInit
{
  public name = new FormControl('', [
    Validators.required,
    Validators.minLength(Validation.MinNameLength),
    Validators.maxLength(Validation.MaxNameLength),
  ]);

  private formBuilder = inject(FormBuilder);
  private voterService = inject(VotersService);

  constructor() {
    super();
    this.form = this.createForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();
    this.initReactiveBindings();

    this.onSubmit((createVoter) => {
      this.voterService.create(createVoter);
    });

    this.onSubmitSuccess((result: IApiResult) => {
      this.toastr.success(result.message);
    });

    this.onSubmitError();
  }

  override initReactiveBindings(): void {
    this.bind(this.voterService.createVoterSuccess$, this.submitSuccess$);

    this.bind(this.voterService.createVoterError$, this.submitError$);
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      name: this.name,
    });
  }
}
