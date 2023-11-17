import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseFormComponent } from '../../shared/components/base/base-form.component';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { SubmitButtonComponent } from '../../shared/components/submit-button/submit-button.component';
import { ValidationMessagesDirective } from '../../core/directives/validation-messages.directive';
import { ElectionsService } from '../../election/services/elections.service';
import { UuidValidatorUtils } from '../../core/utils/uuid-validator.utils';
import { RouteNames } from '../../core/constants/route-names';

@Component({
  selector: 'app-join-election',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SubmitButtonComponent,
    ValidationMessagesDirective,
  ],
  templateUrl: './join-election.component.html',
})
export class JoinElectionComponent
  extends BaseFormComponent<{ roomCode: string }>
  implements OnInit
{
  public roomCode = new FormControl('', [Validators.required]);

  private formBuilder = inject(FormBuilder);
  private router = inject(Router);
  private electionService = inject(ElectionsService);
  private uuidValdator = inject(UuidValidatorUtils);

  constructor() {
    super();
    this.form = this.createForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.onSubmit().subscribe((roomCode) => {
      const roomCodeTrimmed = roomCode.roomCode.trim();
      const isValid = this.uuidValdator.isValid(roomCodeTrimmed);
      if (isValid)
        this.router.navigateByUrl(
          `/${RouteNames.ElectionRoom}/${roomCodeTrimmed}`,
        );
      else this.toastr.error("Room with provided room key doesn't exist.");
    });
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      roomCode: this.roomCode,
    });
  }
}
