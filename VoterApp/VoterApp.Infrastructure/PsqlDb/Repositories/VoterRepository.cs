using VoterApp.Application.Contracts;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.PsqlDb.Repositories;

public class VoterRepository : IVoterRepository
{
    public Task<Voter?> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Voter>> GetAll()
    {
        throw new NotImplementedException();
    }

    // public Task<int> Create(CreateVoterCommand createCommand)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task Update(UpdateVoterCommand updateCommand)
    // {
    //     throw new NotImplementedException();
    // }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}