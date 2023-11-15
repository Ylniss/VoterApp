import { ICandidate } from './candidate';
import { IVoter } from './voter';
import { IApiResult } from '../../core/models/api-result';

export interface IElection {
  id: number;
  topic: string;
  roomCode: string;
  voters: IVoter[];
  candidates: ICandidate[];
}

export interface ICreateElection {
  topic: string;
}

export interface ICreateElectionResult {
  apiResult: IApiResult;
  election: IElection;
}
