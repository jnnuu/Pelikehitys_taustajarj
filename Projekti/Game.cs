using System;

public class Game
{
    public Player[] _players;
    public int _numberOfPlayers;
    public Game(Player[] players, int numberOfPlayers)
    {
        _numberOfPlayers = numberOfPlayers;
        _players = new Player[_numberOfPlayers];
        _players = players;
    }

    public Guid id_game;

}