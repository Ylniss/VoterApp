export interface ICandidate {
  id: number;
  name: string;
  votes: number;
  electionId: number;
}

export interface ICreateCandidate {
  name: string;
}

export interface IUpdateCandidate {
  name: string;
}
