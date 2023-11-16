import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseFormComponent } from '../../../../shared/components/base/base-form.component';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Validation } from '../../../../core/constants/validation';
import { ElectionsService } from '../../../services/elections.service';
import { switchMap } from 'rxjs';
import {
  ICandidate,
  IUpdateCandidate,
} from '../../../../shared/models/candidate';
import { CandidateService } from '../../../services/candidate.service';
import { SubmitButtonComponent } from '../../../../shared/components/submit-button/submit-button.component';
import { ValidationMessagesDirective } from '../../../../core/directives/validation-messages.directive';

@Component({
  selector: 'app-update-candidate',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SubmitButtonComponent,
    ReactiveFormsModule,
    ValidationMessagesDirective,
  ],
  templateUrl: './update-candidate.component.html',
})
export class UpdateCandidateComponent extends BaseFormComponent<IUpdateCandidate> {
  @Input() candidate!: ICandidate;

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

    if (!this.candidate)
      throw new Error('UpdateCandidateComponent candidate is not initialized.');

    this.onSubmit()
      .pipe(
        switchMap((updateCandidate) =>
          this.candidateService.update(this.candidate.id, updateCandidate),
        ),
        switchMap(() =>
          this.electionService.loadByElectionId(this.candidate.electionId),
        ),
      )
      .subscribe();
  }

  protected override populateForm(updateCandidate: IUpdateCandidate): void {
    this.formsService.populateControl(this.name, updateCandidate.name);
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      name: this.name,
    });
  }
}
