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
import { ElectionsService } from '../../services/elections.service';
import { RoomCodeUrlGrabberService } from '../../services/room-code-url-grabber.service';
import { map, switchMap } from 'rxjs';

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
  private electionService = inject(ElectionsService);
  private roomCodeUrlGrabber = inject(RoomCodeUrlGrabberService);

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
        switchMap((createVoter) => this.voterService.create(createVoter)),
      )
      .subscribe((result) => {
        this.toastr.success(result.message);
      });
  }

  private addElectionIdToVoterFromLocalStorage(createVoter: ICreateVoter) {
    const roomCode = this.roomCodeUrlGrabber.getRoomCode();
    if (roomCode) {
      const electionId = localStorage.getItem(roomCode)?.toString();
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
