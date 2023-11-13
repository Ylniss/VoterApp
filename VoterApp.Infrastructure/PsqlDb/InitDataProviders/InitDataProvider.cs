namespace VoterApp.Infrastructure.PsqlDb.InitDataProviders;

public class InitDataProvider : IInitDataProvider
{
    public IEnumerable<string> GetSchemaQueries()
        => new[]
        {
            @"    
                    CREATE TABLE IF NOT EXISTS Elections (
                        Id SERIAL PRIMARY KEY,
                        Topic VARCHAR NOT NULL,
                        Archived BOOLEAN NOT NULL,
                        RoomNumber UUID NOT NULL UNIQUE DEFAULT gen_random_uuid ()
                );",
            @"               
                    CREATE TABLE IF NOT EXISTS Candidates (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR NOT NULL,
                        ElectionId INTEGER NOT NULL REFERENCES Elections(Id),
                        UNIQUE (ElectionId, Name)
                );",
            @"    
                    CREATE TABLE IF NOT EXISTS Voters (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR NOT NULL,
                        ElectionId INTEGER NOT NULL REFERENCES Elections(Id),
                        VotedCandidateId INTEGER REFERENCES Candidates(Id) ON DELETE SET NULL,
                        KeyPhrase VARCHAR NOT NULL,
                        UNIQUE (ElectionId, KeyPhrase)
                );"
        };


    public IEnumerable<string> GetInsertQueries()
        => new[]
        {
            @"
                        INSERT INTO Elections(Topic, Archived, RoomNumber)
                        VALUES 
                            ('Choose your man', false, 'c7f8b63d-4ca7-41f8-bd28-54ff5d41dc13'),
                            ('Select yo characta', false, '4de96b78-c5d8-4cad-8c57-42ad89b4c9b3');",
            @"
                        INSERT INTO Candidates(Name, ElectionId)
                        VALUES 
                            ('Bogdan', 1),
                            ('Gobdan', 2),
                            ('Dobgan', 1),
                            ('Topgun', 2);",
            @"
                        INSERT INTO Voters(Name, ElectionId, VotedCandidateId, KeyPhrase) 
                        VALUES 
                            ('Chillman', 1, 1, '123'),
                            ('Lilchan',1, 1, '321'),
                            ('Nalchil',1, 3, '111'),
                            ('Chinchin',2, null, '997'),
                            ('Inchin',2, null, 'sikret'),
                            ('BruhMan',2, null, 'elo');"
        };
}