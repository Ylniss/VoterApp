namespace VoterApp.Application.Features.Voters.Dtos;

public record VoterDto(int Id, string Name, int ElectionId, string KeyPhrase, bool HasVoted);

public record VoterPublicDto(string Name, bool HasVoted);

public record CreateVoterDto(string Name, int ElectionId);

public record UpdateVoterDto(string Name);