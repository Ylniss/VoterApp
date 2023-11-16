import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseFormComponent } from '../../shared/components/base/base-form.component';
import { IVote } from '../../shared/models/voter';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Validation } from '../../core/constants/validation';
import { ElectionsService } from '../services/elections.service';
import { map, switchMap } from 'rxjs';
import { VotersService } from '../services/voters.service';
import { SubmitButtonComponent } from '../../shared/components/submit-button/submit-button.component';
import { ValidationMessagesDirective } from '../../core/directives/validation-messages.directive';
import {
  NgbDropdown,
  NgbDropdownMenu,
  NgbDropdownToggle,
} from '@ng-bootstrap/ng-bootstrap';
import { UUID } from 'crypto';

@Component({
  selector: 'app-vote',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SubmitButtonComponent,
    ReactiveFormsModule,
    ValidationMessagesDirective,
    NgbDropdown,
    NgbDropdownToggle,
    NgbDropdownMenu,
  ],
  templateUrl: './vote.component.html',
  styleUrl: './vote.component.scss',
})
export class VoteComponent extends BaseFormComponent<IVote> {
  @Input() roomCode!: UUID;

  public candidateId = new FormControl(0, [Validators.required]);

  public voterId = new FormControl(0, [Validators.required]);

  public keyPhrase = new FormControl('', [
    Validators.required,
    Validators.minLength(Validation.KeyPhraseLength),
    Validators.maxLength(Validation.KeyPhraseLength),
  ]);
  public electionService = inject(ElectionsService);
  public voterDropdownDefaultText: string = 'I am';
  public candidateDropdownDefaultText: string = 'I vote for';
  public selectedVoterName: string = this.voterDropdownDefaultText;
  public selectedCandidateName: string = this.candidateDropdownDefaultText;
  private formBuilder = inject(FormBuilder);
  private voterService = inject(VotersService);

  constructor() {
    super();
    this.form = this.createForm();
  }

  override ngOnInit(): void {
    super.ngOnInit();

    if (!this.roomCode)
      throw new Error('VoteComponent roomCode not initialized.');

    this.onSubmit()
      .pipe(
        switchMap((vote) => this.voterService.vote(vote)),
        switchMap((result) =>
          this.electionService.loadByRoomCode(this.roomCode),
        ),
      )
      .subscribe(() => {
        this.selectedVoterName = this.voterDropdownDefaultText;
        this.selectedCandidateName = this.candidateDropdownDefaultText;
      });
  }

  public selectVoter(voterName: string) {
    this.electionService.electionPublic$
      .pipe(
        map((election) => election.voters),
        map((voters) => voters.find((voter) => voter.name === voterName)),
      )
      .subscribe((voter) => {
        if (voter) {
          this.voterId.setValue(voter.id);
          this.selectedVoterName = voterName;
        }
      });
  }

  public selectCandidate(candidateName: string) {
    this.electionService.electionPublic$
      .pipe(
        map((election) => election.candidates),
        map((candidates) =>
          candidates.find((candidate) => candidate.name === candidateName),
        ),
      )
      .subscribe((candidate) => {
        if (candidate) {
          this.candidateId.setValue(candidate.id);
          this.selectedCandidateName = candidateName;
        }
      });
  }

  private createForm(): FormGroup {
    return this.formBuilder.group({
      candidateId: this.candidateId,
      voterId: this.voterId,
      keyPhrase: this.keyPhrase,
    });
  }
}
