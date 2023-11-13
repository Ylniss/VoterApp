using VoterApp.Domain.Common;
using VoterApp.Domain.ValueObjects;

namespace VoterApp.Domain.Entities;

public class Voter : BaseEntity
{
    public Voter(string name, Election election)
    {
        Name = name;
        Election = election;
        KeyPhrase = new KeyPhrase().Key;
    }

    private Voter()
    {
    }

    public string Name { get; set; }

    public Candidate? VotedCandidate { get; set; }

    public Election Election { get; set; }

    public string KeyPhrase { get; set; }

    public bool HasVoted => VotedCandidate is not null;

    public void Vote(Candidate candidate)
    {
        candidate.Voters.Add(this);
        VotedCandidate = candidate;
    }
}