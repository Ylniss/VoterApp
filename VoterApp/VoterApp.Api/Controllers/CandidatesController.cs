using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VoterApp.Api.ErrorResponses;
using VoterApp.Application.Common.Responses;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.DeleteCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidateName;
using VoterApp.Application.Features.Candidates.Dtos;
using VoterApp.Application.Features.Candidates.Queries.GetAllCandidates;
using VoterApp.Application.Features.Candidates.Queries.GetCandidate;

namespace VoterApp.Api.Controllers;

public class CandidatesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CandidatesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    // GET api/candidates/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidateDto>> Get(int id)
    {
        var candidate = await _mediator.Send(new GetCandidateQuery(id));
        return Ok(candidate);
    }

    // GET api/candidates
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<IEnumerable<CandidateDto>>> GetAll()
    {
        var candidates = await _mediator.Send(new GetAllCandidatesQuery());
        return Ok(candidates);
    }

    // POST api/candidates
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse>> Create([FromBody] CreateCandidateDto candidate)
    {
        var command = _mapper.Map<CreateCandidateCommand>(candidate);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    // PUT api/candidates/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommandResponse>> Update(int id, [FromBody] UpdateCandidateNameDto candidate)
    {
        var command = _mapper.Map<UpdateCandidateNameCommand>(candidate);
        command = command with { Id = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE api/candidates/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommandResponse>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCandidateCommand(id));
        return Ok(result);
    }
}