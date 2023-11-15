import { ICandidate, ICandidatePublic } from './candidate';
import { IVoter, IVoterPublic } from './voter';
import { IApiResult } from '../../core/models/api-result';

export interface IElection {
  id: number;
  topic: string;
  archived: boolean;
  roomCode: string;
  voters: IVoter[];
  candidates: ICandidate[];
}

export interface IElectionPublic {
  topic: string;
  roomCode: string;
  archived: boolean;
  voters: IVoterPublic[];
  candidates: ICandidatePublic[];
}

export interface ICreateElection {
  topic: string;
}

export interface ICreateElectionResult {
  apiResult: IApiResult;
  election: IElection;
}
