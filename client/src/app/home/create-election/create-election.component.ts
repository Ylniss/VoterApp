import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ElectionsService } from '../../election/services/elections.service';
import { BaseFormComponent } from '../../shared/components/base/base-form.component';
import {
  ICreateElection,
  ICreateElectionResult,
} from '../../shared/models/election';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { SubmitButtonComponent } from '../../shared/components/submit-button/submit-button.component';
import { ValidationMessagesDirective } from '../../core/directives/validation-messages.directive';
import { Validation } from '../../core/constants/validation';
import { NavigationExtras, Router } from '@angular/router';

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
export class CreateElectionComponent
  extends BaseFormComponent<ICreateElection>
  implements OnInit
{
  public topic = new FormControl('', [
    Validators.required,
    Validators.minLength(Validation.MinTopicLength),
    Validators.maxLength(Validation.MaxTopicLength),
  ]);

  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  private electionsService = inject(ElectionsService);

  constructor() {
    super();
    this.form = this.createForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();
    this.initReactiveBindings();

    this.onSubmit((createElection) => {
      this.electionsService.create(createElection);
    });

    this.onSubmitSuccess((result: ICreateElectionResult) => {
      this.toastr.success(result.apiResult.message);
      this.navigateToElectionRoomWithElectionId(result);
    });

    this.onSubmitError();
  }

  override initReactiveBindings(): void {
    this.bind(
      this.electionsService.createElectionSuccess$,
      this.submitSuccess$,
    );

    this.bind(this.electionsService.createElectionError$, this.submitError$);
  }

  private navigateToElectionRoomWithElectionId(
    result: ICreateElectionResult,
  ): void {
    const navigationExtras: NavigationExtras = {
      state: { electionId: result.apiResult.id },
    };
    this.router.navigateByUrl(
      `/election/${result.election.roomCode}`,
      navigationExtras,
    );
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      topic: this.topic,
    });
  }
}
