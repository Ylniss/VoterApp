using System.Data;
using VoterApp.Application.Features.Elections.Commands.CreateElection;
using VoterApp.Domain.Entities;

namespace VoterApp.Application.Contracts;

public interface IElectionRepository
{
    Task<Election?> Get(int id, IDbTransaction? transaction = null);
    Task<Election?> GetByRoomCode(Guid roomCode, IDbTransaction? transaction = null);
    Task<int> Create(CreateElectionCommand createCommand, IDbTransaction? transaction = null);
}