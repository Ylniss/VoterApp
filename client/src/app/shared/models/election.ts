export interface IElection {
  id: number;
  topic: string;
  roomCode: string;
}

export interface ICreateElectionResponse {
  id: number;
  message: string;
  roomCode: string;
}
