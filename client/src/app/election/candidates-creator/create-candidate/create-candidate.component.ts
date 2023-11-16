import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseFormComponent } from '../../../shared/components/base/base-form.component';
import { ICreateCandidate } from '../../../shared/models/candidate';
import { UUID } from 'crypto';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Validation } from '../../../core/constants/validation';
import { ElectionsService } from '../../services/elections.service';
import { map, mergeMap, switchMap } from 'rxjs';
import { CandidateService } from '../../services/candidate.service';
import { SubmitButtonComponent } from '../../../shared/components/submit-button/submit-button.component';
import { ValidationMessagesDirective } from '../../../core/directives/validation-messages.directive';

@Component({
  selector: 'app-create-candidate',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SubmitButtonComponent,
    ReactiveFormsModule,
    ValidationMessagesDirective,
  ],
  templateUrl: './create-candidate.component.html',
})
export class CreateCandidateComponent
  extends BaseFormComponent<ICreateCandidate>
  implements OnInit
{
  @Input() roomCode!: UUID;

  public name = new FormControl('', [
    Validators.required,
    Validators.minLength(Validation.MinNameLength),
    Validators.maxLength(Validation.MaxNameLength),
  ]);

  private formBuilder = inject(FormBuilder);
  private candidateService = inject(CandidateService);
  private electionService = inject(ElectionsService);

  constructor() {
    super();
    this.form = this.createForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.onSubmit()
      .pipe(
        map((createCandidate) => {
          return this.addElectionIdToCandidateFromLocalStorage(createCandidate);
        }),
        mergeMap((createCandidate) =>
          this.candidateService
            .create(createCandidate)
            .pipe(map(() => createCandidate)),
        ),
        switchMap((result) =>
          this.electionService.loadByElectionId(result.electionId),
        ),
      )
      .subscribe();
  }

  private addElectionIdToCandidateFromLocalStorage(
    createCandidate: ICreateCandidate,
  ) {
    if (this.roomCode) {
      const electionId = localStorage.getItem(this.roomCode)?.toString();
      if (electionId) createCandidate.electionId = Number(electionId);
    }
    return createCandidate;
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      name: this.name,
    });
  }
}
