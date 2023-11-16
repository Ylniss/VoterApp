import { Component, inject, Input, OnInit } from '@angular/core';
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
import { ElectionsService } from '../../services/elections.service';
import { map, mergeMap, switchMap } from 'rxjs';
import { UUID } from 'crypto';

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
})
export class CreateVoterComponent
  extends BaseFormComponent<ICreateVoter>
  implements OnInit
{
  @Input() roomCode!: UUID;

  public name = new FormControl('', [
    Validators.required,
    Validators.minLength(Validation.MinNameLength),
    Validators.maxLength(Validation.MaxNameLength),
  ]);

  private formBuilder = inject(FormBuilder);
  private voterService = inject(VotersService);
  private electionService = inject(ElectionsService);

  constructor() {
    super();
    this.form = this.createForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.onSubmit()
      .pipe(
        map((createVoter) => {
          return this.addElectionIdToVoterFromLocalStorage(createVoter);
        }),
        mergeMap((createVoter) =>
          this.voterService.create(createVoter).pipe(map(() => createVoter)),
        ),
        switchMap((result) =>
          this.electionService.loadByElectionId(result.electionId),
        ),
      )
      .subscribe();
  }

  private addElectionIdToVoterFromLocalStorage(createVoter: ICreateVoter) {
    if (this.roomCode) {
      const electionId = localStorage.getItem(this.roomCode)?.toString();
      if (electionId) createVoter.electionId = Number(electionId);
    }
    return createVoter;
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      name: this.name,
    });
  }
}
