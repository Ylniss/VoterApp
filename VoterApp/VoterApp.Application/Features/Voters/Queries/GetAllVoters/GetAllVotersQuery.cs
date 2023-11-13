using AutoMapper;
using MediatR;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Voters.Dtos;

namespace VoterApp.Application.Features.Voters.Queries.GetAllVoters;

public record GetAllVotersQuery : IRequest<IEnumerable<VoterDto>>;

public class GetAllVotersQueryHandler : IRequestHandler<GetAllVotersQuery, IEnumerable<VoterDto>>
{
    private readonly IMapper _mapper;
    private readonly IVoterRepository _voterRepository;

    public GetAllVotersQueryHandler(IMapper mapper, IVoterRepository voterRepository)
    {
        _mapper = mapper;
        _voterRepository = voterRepository;
    }

    public async Task<IEnumerable<VoterDto>> Handle(GetAllVotersQuery request,
        CancellationToken cancellationToken)
    {
        var voters = await _voterRepository.GetAll();

        return _mapper.Map<IEnumerable<VoterDto>>(voters);
    }
}