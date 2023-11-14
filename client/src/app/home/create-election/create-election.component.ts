import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateElectionService } from './create-election.service';
import { BaseFormComponent } from '../../shared/components/base/base-form.component';
import { ICreateElection } from '../../shared/models/election';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { LoadingIndicatorService } from '../../shared/services/loading-indicator.service';
import { FormsService } from '../../shared/services/forms.service';
import { SubmitButtonComponent } from '../../shared/components/submit-button/submit-button.component';
import { Messages } from '../../core/enums/messages';
import { ValidationMessagesDirective } from '../../core/directives/validation-messages.directive';
import { Validation } from '../../core/enums/validation';

@Component({
  selector: 'app-create-election',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SubmitButtonComponent,
    ValidationMessagesDirective,
  ],
  templateUrl: './create-election.component.html',
})
export class CreateElectionComponent extends BaseFormComponent<ICreateElection> {
  public topic = new FormControl('', [
    Validators.required,
    Validators.minLength(Validation.MinTopicLength),
    Validators.maxLength(Validation.MaxTopicLength),
  ]);

  override formsService = inject(FormsService);
  override loadingIndicatorService = inject(LoadingIndicatorService);
  protected readonly Messages = Messages;
  protected readonly Validation = Validation;
  private createElectionService = inject(CreateElectionService);
  private formBuilder = inject(FormBuilder);

  constructor() {
    super();
    this.form = this.createForm();
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      Topic: this.topic,
    });
  }
}
