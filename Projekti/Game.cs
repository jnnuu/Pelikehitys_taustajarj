using System;
using System.Collections.Generic;

public class Game
{
    public List<Player> _players { get; set; }
    public int _numberOfPlayers { get; set; }
    public String Id { get; set; }
    public Game()
    {
        _players = new List<Player>();
    }


}