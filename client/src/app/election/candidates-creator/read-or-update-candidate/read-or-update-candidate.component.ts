import {
  AfterViewInit,
  Component,
  DestroyRef,
  inject,
  Input,
  OnInit,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseDynamicContentComponent } from '../../../shared/components/base/base-dynamic-content.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReadOrUpdateMode } from '../../../shared/services/base-component-modes.service';
import { CandidatesCreatorComponentModesService } from '../../services/candidates-creator-component-modes.service';
import { ReadCandidateCreatorComponent } from './read-candidate-creator/read-candidate-creator.component';
import { ICandidate } from '../../../shared/models/candidate';
import { UpdateCandidateComponent } from './update-candidate/update-candidate.component';

@Component({
  selector: 'app-read-or-update-candidate',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-or-update-candidate.component.html',
})
export class ReadOrUpdateCandidateComponent
  extends BaseDynamicContentComponent
  implements OnInit, AfterViewInit
{
  @Input() candidate!: ICandidate;
  @ViewChild('dynamicReadOrUpdateCandidateCreatorContent', {
    read: ViewContainerRef,
  })
  override viewContainerRef!: ViewContainerRef;
  candidatesCreatorComponentModesService = inject(
    CandidatesCreatorComponentModesService,
  );
  protected destroyedRef = inject(DestroyRef);
  private readLoaded: boolean = false;
  private updateLoaded: boolean = false;

  public loadReadComponent(): void {
    if (this.readLoaded) return;

    this.loadComponent<ReadCandidateCreatorComponent>(
      ReadCandidateCreatorComponent,
      (readCandidateCreatorComponent) => {
        readCandidateCreatorComponent.candidate = this.candidate;
      },
    );
    this.readLoaded = true;
    this.updateLoaded = false;
  }

  public loadUpdateComponent(): void {
    if (this.updateLoaded) return;

    this.loadComponent<UpdateCandidateComponent>(
      UpdateCandidateComponent,
      (updateCandidateComponent) => {
        updateCandidateComponent.candidate = this.candidate;
        updateCandidateComponent.initEntity = {
          name: this.candidate.name,
        };
      },
    );

    this.readLoaded = false;
    this.updateLoaded = true;
  }

  ngOnInit(): void {
    if (!this.candidate)
      throw new Error(
        'ReadOrUpdateCandidateComponent candidate is not initialized.',
      );
  }

  ngAfterViewInit(): void {
    // hack: put it and the end of JS event queue to avoid errors
    setTimeout(() => {
      this.candidatesCreatorComponentModesService.modes$
        .pipe(takeUntilDestroyed(this.destroyedRef))
        .subscribe((modes) => {
          const mode = modes.get(this.candidate.id);

          if (mode === ReadOrUpdateMode.UPDATE) this.loadUpdateComponent();
          else if (mode === ReadOrUpdateMode.READ) this.loadReadComponent();
        });

      this.candidatesCreatorComponentModesService.setMode(
        this.candidate.id,
        ReadOrUpdateMode.READ,
      );
    }, 0);
  }
}
