namespace VoterApp.Application.Features.Elections.Dtos;

public record CreateElectionCommandResponse(int Id, string Message, Guid RoomCode);