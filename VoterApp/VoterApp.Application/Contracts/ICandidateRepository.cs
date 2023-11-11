using VoterApp.Domain.Entities;

namespace VoterApp.Application.Contracts;

public interface ICandidateRepository
{
    Task<Candidate?> Get(int id);

    Task<IEnumerable<Voter>> GetAll();

    //Task<int> Create(CreateCandidateCommand createCommand);

    //Task Update(UpdateCandidateCommand updateCommand);

    Task Delete(int id);
}