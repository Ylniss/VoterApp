using System.Data;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Contracts;

public interface ICandidateRepository
{
    Task<Candidate?> Get(int id, IDbTransaction? transaction = null);

    Task<IEnumerable<Candidate>> GetAll(IDbTransaction? transaction = null);
    Task<IEnumerable<Candidate>> GetAll(int electionId, IDbTransaction? transaction = null);

    Task<int> Create(CreateCandidateCommand createCommand, IDbTransaction? transaction = null);

    Task Update(UpdateCandidateCommand updateCommand, IDbTransaction? transaction = null);

    Task Delete(int id, IDbTransaction? transaction = null);
}