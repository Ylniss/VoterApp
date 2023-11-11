using VoterApp.Domain.Common;

namespace VoterApp.Domain.Entities;

public class Candidate : BaseEntity
{
    public string Name { get; init; }
    public List<Voter> Voters { get; } = new List<Voter>();

    public Candidate(string name)
    {
        Name = name;
    }

    public int GetNumberOfVotes() => Voters.Count;
}