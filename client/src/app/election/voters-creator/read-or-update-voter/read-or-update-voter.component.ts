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
import { IVoter } from '../../../shared/models/voter';
import { BaseDynamicContentComponent } from '../../../shared/components/base/base-dynamic-content.component';
import { ReadVoterCreatorComponent } from './read-voter-creator/read-voter-creator.component';
import { UpdateVoterComponent } from './update-voter/update-voter.component';
import { VotersCreatorComponentModesService } from '../../services/voters-creator-component-modes.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReadOrUpdateMode } from '../../../shared/services/base-component-modes.service';

@Component({
  selector: 'app-read-or-update-voter',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './read-or-update-voter.component.html',
})
export class ReadOrUpdateVoterComponent
  extends BaseDynamicContentComponent
  implements OnInit, AfterViewInit
{
  @Input() voter!: IVoter;
  @ViewChild('dynamicReadOrUpdateVoterCreatorContent', {
    read: ViewContainerRef,
  })
  override viewContainerRef!: ViewContainerRef;
  votersCreatorComponentModesService = inject(
    VotersCreatorComponentModesService,
  );
  protected destroyedRef = inject(DestroyRef);
  private currentDisplayedVoterId: number = 0;
  private readLoaded: boolean = false;
  private updateLoaded: boolean = false;

  public loadReadComponent(): void {
    if (this.readLoaded) return;

    this.loadComponent<ReadVoterCreatorComponent>(
      ReadVoterCreatorComponent,
      (readVoterCreatorComponent) => {
        readVoterCreatorComponent.voter = this.voter;
      },
    );
    this.readLoaded = true;
    this.updateLoaded = false;
  }

  public loadUpdateComponent(): void {
    if (this.updateLoaded) return;

    this.loadComponent<UpdateVoterComponent>(
      UpdateVoterComponent,
      (updateVoterComponent) => {
        updateVoterComponent.voter = this.voter;
      },
    );

    this.readLoaded = false;
    this.updateLoaded = true;
  }

  ngOnInit(): void {
    if (!this.voter)
      throw new Error('ReadOrUpdateVoterComponent voter is not initialized.');
  }

  ngAfterViewInit(): void {
    // hack: put it and the end of JS event queue to avoid errors
    setTimeout(() => {
      this.votersCreatorComponentModesService.modes$
        .pipe(takeUntilDestroyed(this.destroyedRef))
        .subscribe((modes) => {
          const mode = modes.get(this.voter.id);

          if (mode === ReadOrUpdateMode.UPDATE) this.loadUpdateComponent();
          else if (mode === ReadOrUpdateMode.READ) this.loadReadComponent();
        });

      this.votersCreatorComponentModesService.setMode(
        this.voter.id,
        ReadOrUpdateMode.READ,
      );
    }, 0);
  }
}
