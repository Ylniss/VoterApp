using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Features.Elections.Commands.CreateElection;
using VoterApp.Application.Features.Elections.Dtos;
using VoterApp.Application.Features.Elections.Queries.GetElection;
using VoterApp.Application.Features.Elections.Queries.IsRoomCodeAndElectionIdPairValid;

namespace VoterApp.Api.Controllers;

public class ElectionsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ElectionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    // GET api/elections/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ElectionDto>> Get(int id)
    {
        var election = await _mediator.Send(new GetElectionQuery(id));
        return Ok(election);
    }

    // GET api/elections/roomcode/42e13ac3-03a2-4c23-8c77-c866e6f318b8
    [HttpGet("roomcode/{roomCode:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ElectionDto>> GetByRoomCode(Guid roomCode)
    {
        var election = await _mediator.Send(new GetElectionByRoomCodeQuery(roomCode));
        return Ok(election);
    }

    // GET api/elections/roomcode/42e13ac3-03a2-4c23-8c77-c866e6f318b8/5
    [HttpGet("roomcode/{roomCode:guid}/{electionId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<bool>> IsRoomCodeAndElectionIdPairValid(Guid roomCode, int electionId)
    {
        var isValid = await _mediator.Send(new IsRoomCodeAndElectionIdPairValidQuery(roomCode, electionId));
        return Ok(isValid);
    }

    // POST api/elections
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse>> Create([FromBody] CreateElectionDto election)
    {
        var command = _mapper.Map<CreateElectionCommand>(election);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
}