using System;
using System.Collections.Generic;
using System.Threading.Tasks;



public interface IRepository
{
    Task<Game> StartNewGame(Game game);
    Task<Player> CreateAPlayer(Player player, String id);
    Task<Player> GetPlayer(String id);
    Task<Player> AddScore(String id, int score, Combination combination);
    Task<String> GetFreeFields(String id);
    Task<int> GetScore(String id);
    Task<Player> DeleteScore(String id, Combination combination);

    Task<Player> GetWinner(String id_game);
    Task<Game[]> GetGames();
    Task<String> Help();
    Task<String> GetScoreboard(String id);

    Task<Player> AddRandomValuesAll(String id);
    Task<Player> ResetFields(String id);
    Task<Player> CheckForBonus(String id);

    Task<String> NukeAllGames();
    Task<String> NukeGame(String id_game);



}