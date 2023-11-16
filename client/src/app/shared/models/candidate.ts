export interface ICandidate {
  id: number;
  name: string;
  votes: number;
  electionId: number;
}

export interface ICandidatePublic {
  id: number;
  name: string;
  votes: number;
}

export interface ICreateCandidate {
  name: string;
  electionId: number;
}

export interface IUpdateCandidate {
  name: string;
}
