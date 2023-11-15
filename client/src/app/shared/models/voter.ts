export interface IVoter {
  id: number;
  name: string;
  hasVoted: boolean;
  keyPhrase: string;
  electionId: number;
}

export interface IVoterPublic {
  name: string;
  hasVoted: boolean;
}

export interface ICreateVoter {
  name: string;
  electionId: number;
}

export interface IUpdateVoter {
  name: string;
}

export interface IVote {
  voterId: number;
  candidateId: number;
  keyPhrase: string;
}
