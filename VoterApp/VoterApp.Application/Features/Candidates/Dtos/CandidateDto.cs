namespace VoterApp.Application.Features.Candidates.Dtos;

public record CandidateDto(int Id, string Name);

public record CreateCandidateDto(string Name);

public record UpdateCandidateNameDto(string Name);