using System;

public class Player
{
    public Scoreboard scoreboard { get; set; }
    public String Id { get; set; }
    public String name { get; set; }

    public Player()
    {
        scoreboard = new Scoreboard();
    }


}