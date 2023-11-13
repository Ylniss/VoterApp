using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Features.Voters.Commands.CreateVoter;
using VoterApp.Application.Features.Voters.Commands.DeleteVoter;
using VoterApp.Application.Features.Voters.Commands.UpdateVoterName;
using VoterApp.Application.Features.Voters.Commands.Vote;
using VoterApp.Application.Features.Voters.Dtos;
using VoterApp.Application.Features.Voters.Queries.GetAllVoters;
using VoterApp.Application.Features.Voters.Queries.GetVoter;

namespace VoterApp.Api.Controllers;

public class VotersController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public VotersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    // GET api/voters/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VoterDto>> Get(int id)
    {
        var voter = await _mediator.Send(new GetVoterQuery(id));
        return Ok(voter);
    }

    // GET api/voters
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<VoterDto>>> GetAll()
    {
        var voters = await _mediator.Send(new GetAllVotersQuery());
        return Ok(voters);
    }

    // POST api/voters
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse>> Create([FromBody] CreateVoterDto voter)
    {
        var command = _mapper.Map<CreateVoterCommand>(voter);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    // PUT api/voters/vote
    [HttpPut("vote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse>> Vote([FromBody] VoteDto vote)
    {
        var command = _mapper.Map<VoteCommand>(vote);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT api/voters/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse>> Update(int id, [FromBody] UpdateVoterDto voter)
    {
        var voterElectionId = (await _mediator.Send(new GetVoterQuery(id))).ElectionId;

        var command = _mapper.Map<UpdateVoterCommand>(voter);
        command = command with { Id = id, ElectionId = voterElectionId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE api/voters/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommandResponse>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteVoterCommand(id));
        return Ok(result);
    }
}