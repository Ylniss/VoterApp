﻿using VoterApp.Application.Features.Candidates.Dtos;
using VoterApp.Application.Features.Voters.Dtos;

namespace VoterApp.Application.Features.Elections.Dtos;

public record ElectionDto(int Id, string Topic, bool Archived, Guid RoomNumber, IEnumerable<CandidateDto> Candidates,
    IEnumerable<VoterDto> Voters);

public record CreateElectionDto(string Topic);