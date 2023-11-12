using VoterApp.Domain.Common;

namespace VoterApp.Domain.Entities;

public class Candidate : BaseEntity
{
    public Candidate(string name, Election election)
    {
        Name = name;
        Election = election;
    }

    private Candidate()
    {
    }

    public string Name { get; set; }
    public List<Voter> Voters { get; } = new();

    public Election Election { get; set; }
}