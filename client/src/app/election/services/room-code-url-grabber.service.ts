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
      if (this.uuidValidator.isValid(roomCode)) {
        return roomCode as UUID;
      } else {
        throw new Error("Couldn't obtain room code. Invalid url.");
      }
    }
    return null;
  }
}
