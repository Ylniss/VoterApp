using VoterApp.Application.Contracts;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.Respositories;

public class CandidateRepository : ICandidateRepository
{
    public Task<Candidate?> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Voter>> GetAll()
    {
        throw new NotImplementedException();
    }

    // public Task<int> Create(CreateCandidateCommand createCommand)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task Update(UpdateCandidateCommand updateCommand)
    // {
    //     throw new NotImplementedException();
    // }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}