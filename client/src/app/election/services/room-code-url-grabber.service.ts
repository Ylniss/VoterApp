import { Inject, inject, Injectable, PLATFORM_ID } from '@angular/core';
import { UuidValidatorUtils } from '../../core/utils/uuid-validator.utils';
import { UUID } from 'crypto';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class RoomCodeUrlGrabberService {
  uuidValidator = inject(UuidValidatorUtils);

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  public getRoomCode(): UUID | null {
    if (isPlatformBrowser(this.platformId)) {
      const url = window.location.href;
      const segments = url.split('/');
      const roomCode = segments.pop() || '';
      return this.uuidValidator.isValid(roomCode) ? (roomCode as UUID) : null;
    } else return null;
  }
}
