namespace VoterApp.Application.Features.Voters.Dtos;

public record VoteDto(int VoterId, int CandidateId, string KeyPhrase);