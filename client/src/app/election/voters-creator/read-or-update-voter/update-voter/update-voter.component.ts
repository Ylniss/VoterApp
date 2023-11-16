import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IUpdateVoter, IVoter } from '../../../../shared/models/voter';
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
import { VotersService } from '../../../services/voters.service';
import { ElectionsService } from '../../../services/elections.service';
import { switchMap } from 'rxjs';
import { SubmitButtonComponent } from '../../../../shared/components/submit-button/submit-button.component';
import { ValidationMessagesDirective } from '../../../../core/directives/validation-messages.directive';

@Component({
  selector: 'app-update-voter',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SubmitButtonComponent,
    ReactiveFormsModule,
    ValidationMessagesDirective,
  ],
  templateUrl: './update-voter.component.html',
})
export class UpdateVoterComponent extends BaseFormComponent<IUpdateVoter> {
  @Input() voter!: IVoter;

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

    if (!this.voter)
      throw new Error('ReadVoterCreatorComponent voter is not initialized.');

    this.onSubmit()
      .pipe(
        switchMap((updateVoter) =>
          this.voterService.update(this.voter.id, updateVoter),
        ),
        switchMap(() =>
          this.electionService.loadByElectionId(this.voter.electionId),
        ),
      )
      .subscribe();
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      name: this.name,
    });
  }
}
