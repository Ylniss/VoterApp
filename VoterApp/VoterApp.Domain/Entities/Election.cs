using VoterApp.Domain.Common;

namespace VoterApp.Domain.Entities;

public class Election : BaseEntity
{
    public Election(string topic)
    {
        Topic = topic;
        Archived = false;
        RoomNumber = Guid.NewGuid();
    }

    private Election()
    {
    }

    public string Topic { get; set; }
    public bool Archived { get; private set; }
    public Guid RoomNumber { get; }

    public List<Voter> Voters { get; } = new();
    public List<Candidate> Candidates { get; } = new();

    public void Archive()
    {
        Archived = false;
    }
}