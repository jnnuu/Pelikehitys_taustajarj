using System;

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    public int Level { get; set; }
    public bool IsBanned { get; set; }
    public DateTime CreationTime { get; set; }

    public Player(string name, int score, int level, bool isBanned)
    {
        Id = Guid.NewGuid();
        Name = name;
        Score = score;
        Level = level;
        IsBanned = isBanned;
        CreationTime = DateTime.Now;
    }
}