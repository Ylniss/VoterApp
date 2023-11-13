using System.Data;
using VoterApp.Application.Features.Voters.Commands.CreateVoter;
using VoterApp.Application.Features.Voters.Commands.UpdateVoterName;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Contracts;

public interface IVoterRepository
{
    Task<Voter?> Get(int id, IDbTransaction? transaction = null);

    Task<IEnumerable<Voter>> GetAll(IDbTransaction? transaction = null);
    Task<IEnumerable<Voter>> GetAll(int electionId, IDbTransaction? transaction = null);

    Task<int> Create(CreateVoterCommand createCommand, IDbTransaction? transaction = null);

    Task Update(UpdateVoterCommand updateCommand, IDbTransaction? transaction = null);

    Task Delete(int id, IDbTransaction? transaction = null);
}