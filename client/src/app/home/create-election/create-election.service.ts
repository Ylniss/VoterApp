import {inject, Injectable} from '@angular/core';
import {ElectionsHttpService} from "../../shared/services/http/elections-http.service";

@Injectable({
  providedIn: 'root'
})
export class CreateElectionService {
  electionsHttpService = inject(ElectionsHttpService);

  constructor() {
  }
}
