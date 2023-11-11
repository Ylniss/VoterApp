using VoterApp.Domain.Entities;

namespace VoterApp.Application.Contracts;

public interface IVoterRepository
{
    Task<Voter?> Get(int id);

    Task<IEnumerable<Voter>> GetAll();

    // Task<int> Create(CreateVoterCommand createCommand);
    //
    // Task Update(UpdateVoterCommand updateCommand);

    Task Delete(int id);
}