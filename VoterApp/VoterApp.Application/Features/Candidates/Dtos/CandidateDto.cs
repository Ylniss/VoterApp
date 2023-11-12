namespace VoterApp.Application.Features.Candidates.Dtos;

public record CandidateDto(int Id, string Name, int Votes);

public record CreateCandidateDto(string Name);

public record UpdateCandidateDto(string Name);