using AutoMapper;
using MediatR;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Voters.Commands.UpdateVoterName;

namespace VoterApp.Application.Features.Voters.Commands.Vote;

public record VoteCommand(int VoterId, int CandidateId, string KeyPhrase) : IRequest<CommandResponse>;

public class VoteCommandHandler : IRequestHandler<VoteCommand, CommandResponse>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;
    private readonly IVoterRepository _voterRepository;

    public VoteCommandHandler(IVoterRepository voterRepository, ICandidateRepository candidateRepository,
        IMapper mapper)
    {
        _voterRepository = voterRepository;
        _mapper = mapper;
        _candidateRepository = candidateRepository;
    }

    public async Task<CommandResponse> Handle(VoteCommand request, CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(request.VoterId);
        var candidate = await _candidateRepository.Get(request.CandidateId);

        voter.Vote(candidate);

        var updateCommand = _mapper.Map<UpdateVoterCommand>(voter);

        await _voterRepository.Update(updateCommand);

        return new CommandResponse(request.VoterId,
            $"{voter.Name} ({voter.Id}) voter for {candidate.Name} ({candidate.Id}).");
    }
}