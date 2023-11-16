using VoterApp.Application.Features.Candidates.Dtos;
using VoterApp.Application.Features.Voters.Dtos;

namespace VoterApp.Application.Features.Elections.Dtos;

public record ElectionDto(int Id, string Topic, bool Archived, Guid RoomCode, IEnumerable<CandidateDto> Candidates,
    IEnumerable<VoterDto> Voters);

public record ElectionPublicDto(string Topic, bool Archived, Guid RoomCode, IEnumerable<CandidatePublicDto> Candidates,
    IEnumerable<VoterPublicDto> Voters);

public record CreateElectionDto(string Topic);