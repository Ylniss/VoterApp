using VoterApp.Domain.Common;

namespace VoterApp.Domain.Entities;

public class Candidate : BaseEntity
{
    public Candidate(string name) => Name = name;

    public string Name { get; init; }
    public List<Voter> Voters { get; } = new();

    public int GetNumberOfVotes() => Voters.Count;
}