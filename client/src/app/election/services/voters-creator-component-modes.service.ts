import { Injectable } from '@angular/core';
import {
  BaseComponentModesService,
  ReadOrUpdateMode,
} from '../../shared/services/base-component-modes.service';

@Injectable({
  providedIn: 'root',
})
export class VotersCreatorComponentModesService extends BaseComponentModesService<ReadOrUpdateMode> {
  override defaultMode = ReadOrUpdateMode.READ;

  constructor() {
    super();
    this.defaultMode = ReadOrUpdateMode.READ;
  }
}
