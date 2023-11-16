import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ElectionsService } from '../../election/services/elections.service';
import { BaseFormComponent } from '../../shared/components/base/base-form.component';
import { ICreateElection } from '../../shared/models/election';
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
import { Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { RouteNames } from '../../core/constants/route-names';

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

    this.onSubmit()
      .pipe(
        switchMap((createElection) =>
          this.electionsService.create(createElection),
        ),
      )
      .subscribe((result) => {
        this.toastr.success(result.apiResult.message);

        localStorage.setItem(
          result.election.roomCode,
          result.apiResult.id.toString(),
        );

        this.router.navigateByUrl(
          `/${RouteNames.ElectionRoom}/${result.election.roomCode}`,
        );
      });
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      topic: this.topic,
    });
  }
}
