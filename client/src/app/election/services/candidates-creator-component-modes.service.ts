import { Injectable } from '@angular/core';
import {
  BaseComponentModesService,
  ReadOrUpdateMode,
} from '../../shared/services/base-component-modes.service';

@Injectable({
  providedIn: 'root',
})
export class CandidatesCreatorComponentModesService extends BaseComponentModesService<ReadOrUpdateMode> {
  override defaultMode = ReadOrUpdateMode.READ;
}
