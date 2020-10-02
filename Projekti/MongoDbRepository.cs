using System;
using System.Collections.Generic;
using System.IO;
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
        var filter = Builders<Game>.Filter.Empty;
        var lista = await _gamesCollection.Find(filter).ToListAsync();

        if ((int)combination == 6 || (int)combination == 7 || (int)combination == 17)
        {
            throw new Exception("wrong field");
        }

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
                        SanityCheck((int)combination, score); // heittää exceptionin jos ei mene läpi, muuten ei toimenpiteitä.
                        if ((int)combination < 6)
                        {
                            newPlayer.scoreboard.scores[(int)Combination.total_up] += score;
                            if (newPlayer.scoreboard.scores[(int)Combination.total_up] >= 63)
                            {
                                newPlayer.scoreboard.scores[(int)Combination.bonus] = 50;
                            }
                        }

                        newPlayer.scoreboard.scores[(int)combination] = score;
                        newPlayer.scoreboard.scores[(int)Combination.total] += score;

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

    public async Task<Player> GetPlayer(String id)
    {
        var filter = Builders<Game>.Filter.Empty;
        List<Game> lista = await _gamesCollection.Find(filter).ToListAsync();
        foreach (var game in lista)
        {
            foreach (var player in game._players)
            {
                if (player.Id == id)
                {
                    return player;
                }
            }
        }
        return null;
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

    public async Task<Game[]> GetGames()
    {
        var filter = Builders<Game>.Filter.Empty;
        List<Game> lista = await _gamesCollection.Find(filter).ToListAsync();
        return lista.ToArray();
    }

    public async Task<Player> GetWinner(String id_game)
    {
        var filter = Builders<Game>.Filter.Eq(g => g.Id, id_game);
        Game foundGame = await _gamesCollection.Find(filter).FirstAsync();
        Player winner = new Player();
        winner.scoreboard.scores[(int)Combination.total] = 0;
        foreach (var player in foundGame._players)
        {
            foreach (var score in player.scoreboard.scores)
            {
                if (score == -1)
                {
                    throw new Exception("Game not finished"); //tähän vois tehdä oman exceptionin
                }
            }
            if (player.scoreboard.scores[(int)Combination.total] > winner.scoreboard.scores[(int)Combination.total])
            {
                winner = player;
            }
        }
        foundGame._winner = winner.name;
        await _gamesCollection.ReplaceOneAsync(filter, foundGame);
        return winner;

    }

    public async Task<Game> StartNewGame(Game game)
    {
        await _gamesCollection.InsertOneAsync(game);
        return game;
    }

    public async Task<String> Help()
    {
        string returnString = await File.ReadAllTextAsync("indeksilista.txt");
        return returnString;
    }
    private void SanityCheck(int combination, int score)
    {
        if (combination < 6) //yläkerta = samojen silmälukujen summa
        {
            if (score % (combination + 1) != 0)
            {
                throw new Exception("wrong value"); // tähän vois tehdä oman exceptionin
            }
        }
        if (combination == 8) // pari = mahd scoret 2 4 6 8 10 12
        {
            if (score % 2 != 0 || score > 12)
            {
                throw new Exception("wrong value");
            }
        }
        if (combination == 9) // 2*pari = mahd scoret 10 12 14 16 18 20 22
        {
            if (score % 2 != 0 || score > 22)
            {
                throw new Exception("wrong value");
            }
        }
        if (combination == 10) // kolmiluku = mahd pisteet 3 6 9 12 15 18
        {
            if (score % 3 != 0 || score > 18)
            {
                throw new Exception("wrong value");
            }
        }
        if (combination == 11) // neliluku = mahd pisteet 4 8 12 16 20 24  
        {
            if (score % 4 != 0 || score > 24)
            {
                throw new Exception("wrong value");
            }
        }
        if (combination == 12) // täyskäsi 
        {
            // TODO: keksi ja toteuta täyskäden toteuttava sääntö
        }
        if (combination == 13) // pieni suora on aina 15 pistettä
        {
            if (score != 15)
            {
                throw new Exception("worng value");
            }
        }
        if (combination == 14) // iso suora on aina 20 pistettä
        {
            if (score != 20)
            {
                throw new Exception("worng value");
            }
        }
        if (combination == 15) // sattuma, viidellä nopalla mahdollisimman suuri tulos, min 5 max 30
        {
            if (score < 5 || score > 30)
            {
                throw new Exception("wrong value");
            }
        }
        if (combination == 16) // yatzy, aina 50 pistettä
        {
            if (score != 50)
            {
                throw new Exception("wrong value");
            }
        }
    }
    public async Task<String> GetScoreboard(String id)
    {
        var player = GetPlayer(id).Result;
        String header = $"Scoreboard for {player.name}";
        String scoreboard = $@"
        ONES:       |{player.scoreboard.scores[0]}|
        TWOS:       |{player.scoreboard.scores[1]}|
        THREES:     |{player.scoreboard.scores[2]}|
        FOURS:      |{player.scoreboard.scores[3]}|
        FIVES:      |{player.scoreboard.scores[4]}|
        SIXES:      |{player.scoreboard.scores[5]}|
        ------------
        total:      |{player.scoreboard.scores[6]}|
        bonus:      |{player.scoreboard.scores[7]}|
        ------------
        PAIR:       |{player.scoreboard.scores[8]}|
        2 PAIRS:    |{player.scoreboard.scores[9]}|
        3OfKind:    |{player.scoreboard.scores[10]}|
        4OfKind:    |{player.scoreboard.scores[11]}|
        Sm.straight:|{player.scoreboard.scores[13]}|
        Lg.straight:|{player.scoreboard.scores[14]}|
        FullHouse:  |{player.scoreboard.scores[12]}|
        Chance:     |{player.scoreboard.scores[15]}|
        Yatzy:      |{player.scoreboard.scores[16]}|
        ------------
        total:      |{player.scoreboard.scores[17]}|";
        return String.Join("\n", header, scoreboard);
    }
}