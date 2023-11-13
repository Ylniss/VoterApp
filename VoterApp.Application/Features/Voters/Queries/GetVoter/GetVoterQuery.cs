using AutoMapper;
using MediatR;
using VoterApp.Application.Common.Exceptions;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Voters.Dtos;

namespace VoterApp.Application.Features.Voters.Queries.GetVoter;

public record GetVoterQuery(int Id) : IRequest<VoterDto>;

public class GetVoterQueryHandler : IRequestHandler<GetVoterQuery, VoterDto>
{
    private readonly IMapper _mapper;
    private readonly IVoterRepository _voterRepository;

    public GetVoterQueryHandler(IMapper mapper, IVoterRepository voterRepository)
    {
        _mapper = mapper;
        _voterRepository = voterRepository;
    }

    public async Task<VoterDto> Handle(GetVoterQuery request, CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.Get(request.Id);

        if (voter is null) throw new NotFoundException(request.Id);

        return _mapper.Map<VoterDto>(voter);
    }
}