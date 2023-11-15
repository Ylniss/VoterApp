export interface IElection {
  id: number;
  topic: string;
  roomCode: string;
}

export interface ICreateElection {
  topic: string;
}

export interface ICreateElectionResult {
  id: number;
  message: string;
  roomCode: string;
}