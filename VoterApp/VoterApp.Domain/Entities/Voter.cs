using VoterApp.Domain.Common;
using VoterApp.Domain.ValueObjects;

namespace VoterApp.Domain.Entities;

public class Voter : BaseEntity
{
    public Voter(string name)
    {
        Name = name;
        KeyPhrase = new KeyPhrase();
    }

    public string Name { get; init; }

    public Candidate? VotedCandidate { get; private set; }

    public bool HasVoted => VotedCandidate is not null;
    public KeyPhrase KeyPhrase { get; }

    public void Vote(Candidate candidate)
    {
        candidate.Voters.Add(this);
        VotedCandidate = candidate;
    }
}