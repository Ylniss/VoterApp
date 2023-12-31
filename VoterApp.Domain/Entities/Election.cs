﻿using VoterApp.Domain.Common;

namespace VoterApp.Domain.Entities;

public class Election : BaseEntity
{
    public Election(string topic)
    {
        Topic = topic;
        Archived = false;
        RoomCode = Guid.NewGuid();
    }

    private Election()
    {
    }

    public string Topic { get; set; }
    public bool Archived { get; private set; }
    public Guid RoomCode { get; }

    public List<Voter> Voters { get; set; } = new();
    public List<Candidate> Candidates { get; set; } = new();

    public void Archive()
    {
        Archived = false;
    }
}