export interface IVoter {
  id: number;
  name: string;
  hasVoted: boolean;
  keyPhrase: string;
  electionId: number;
}

export interface ICreateVoter {
  name: string;
}

export interface IUpdateVoter {
  name: string;
}

export interface IVote {
  voterId: number;
  candidateId: number;
  keyPhrase: string;
}
