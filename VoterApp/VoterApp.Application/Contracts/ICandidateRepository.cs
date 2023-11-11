using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidateName;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Contracts;

public interface ICandidateRepository
{
    Task<Candidate?> Get(int id);

    Task<IEnumerable<Candidate>> GetAll();

    Task<int> Create(CreateCandidateCommand createCommand);

    Task Update(UpdateCandidateNameCommand updateNameCommand);

    Task Delete(int id);
}