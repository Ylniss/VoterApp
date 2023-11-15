export interface ICandidate {
  id: number;
  name: string;
  votes: number;
  electionId: number;
}

export interface ICandidatePublic {
  name: string;
  votes: number;
}

export interface ICreateCandidate {
  name: string;
}

export interface IUpdateCandidate {
  name: string;
}
