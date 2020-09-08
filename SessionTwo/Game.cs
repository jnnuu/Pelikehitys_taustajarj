using System.Collections.Generic;

public class Game<T> where T : IPlayer
{
    private List<T> _players;

    public Game(List<T> players) { _players = players; }

    public T[] GetTop10Players()
    {
        return new T[0];
    }
}