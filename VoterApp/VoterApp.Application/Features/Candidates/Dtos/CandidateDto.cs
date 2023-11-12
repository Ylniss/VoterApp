namespace VoterApp.Application.Features.Candidates.Dtos;

public record CandidateDto(int Id, string Name, int ElectionId, int Votes);

public record CreateCandidateDto(string Name, int ElectionId);

public record UpdateCandidateDto(string Name, int ElectionId);