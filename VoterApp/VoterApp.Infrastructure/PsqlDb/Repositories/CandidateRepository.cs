using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidateName;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.PsqlDb.Repositories;

public class CandidateRepository : ICandidateRepository
{
    public Task<Candidate?> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Candidate>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<int> Create(CreateCandidateCommand createCommand)
    {
        throw new NotImplementedException();
    }

    public Task Update(UpdateCandidateNameCommand updateNameCommand)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}