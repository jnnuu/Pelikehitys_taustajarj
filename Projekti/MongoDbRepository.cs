using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

using MongoDB.Driver;
public enum Combination
{
    ones, twos, threes, fours, fives, sixes, total_up, bonus, one_pair, two_pairs, three_same, four_same, full_house, low_straight, high_straight, chance, yatzy, total
}
public class MongoDbRepository : IRepository
{

    private readonly IMongoCollection<Game> _gamesCollection;
    private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;
    public MongoDbRepository()
    {
        MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("yatzygame");
        _gamesCollection = database.GetCollection<Game>("games");
        _bsonDocumentCollection = database.GetCollection<BsonDocument>("games");
    }

    public async Task<Player> AddScore(String id, int score, Combination combination)
    {
        Console.WriteLine("TÄNNE PÄÄSTIIN 111");
        var filter = Builders<Game>.Filter.Empty;
        Console.WriteLine("TÄNNE PÄÄSTIIN 222");
        var lista = await _gamesCollection.Find(filter).ToListAsync();
        Console.WriteLine("TÄNNE PÄÄSTIIN 333");

        foreach (var game in lista)
        {
            for (int i = 0; i < game._players.Count; i++)      // (var player in game._players)
            {
                if (game._players[i].Id == id)
                {
                    Game newGame = new Game();
                    var filter2 = Builders<Game>.Filter.Eq(g => g.Id, game.Id);
                    newGame = await _gamesCollection.Find(filter2).FirstAsync();
                    Player newPlayer = game._players[i];

                    if (newPlayer.scoreboard.scores[(int)combination] == -1)
                    {
                        newPlayer.scoreboard.scores[(int)combination] = score;
                        newPlayer.scoreboard.scores[(int)Combination.total] += score;

                        if ((int)combination < 6)
                        {
                            Console.WriteLine("TESTII");
                            newPlayer.scoreboard.scores[(int)Combination.total_up] += score;
                            if (newPlayer.scoreboard.scores[(int)Combination.total_up] >= 63)
                            {
                                newPlayer.scoreboard.scores[(int)Combination.bonus] = 50;
                            }
                        }

                        newGame._players[i] = newPlayer;
                        await _gamesCollection.ReplaceOneAsync(filter2, newGame);
                        return newPlayer;
                    }
                    else
                    {
                        throw new Exception(); // tähän voisi vaihtaa oman exceptionin 
                    }

                }
            }
        }
        return null;
    }

    public async Task<Player> CreateAPlayer(Player player, String id)
    {
        var filter = Builders<Game>.Filter.Eq(g => g.Id, id);
        Game foundGame = await _gamesCollection.Find(filter).FirstAsync();
        foundGame._players.Add(player);
        await _gamesCollection.ReplaceOneAsync(filter, foundGame);

        return player;
    }

    public Task<Player> GetPlayer(String id)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetScore(String id)
    {
        var filter = Builders<Game>.Filter.Empty;
        List<Game> lista = await _gamesCollection.Find(filter).ToListAsync();
        foreach (var game in lista)
        {
            foreach (var player in game._players)
            {
                if (player.Id == id)
                {
                    return player.scoreboard.scores[17];
                }
            }
        }
        return -1; // tähän vielä exception myöhemmin mikä otetaan kiinni? 

    }

    public Task<Player> GetWinner(String id_game)
    {
        throw new NotImplementedException();
    }

    public async Task<Game> StartNewGame(Game game)
    {
        await _gamesCollection.InsertOneAsync(game);
        return game;
    }
}