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

[Serializable()]
public class ScoreOutOfBoundsException : System.Exception
{
    public ScoreOutOfBoundsException() : base(String.Format("Score out of bounds")) { }
    public ScoreOutOfBoundsException(string message) : base(String.Format("Score out of bounds: ", message)) { }
    public ScoreOutOfBoundsException(string message, System.Exception inner) : base(String.Format("Score out of bounds: ", message, inner)) { }

    protected ScoreOutOfBoundsException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class GameNotFoundException : System.Exception
{
    public GameNotFoundException() : base(String.Format("Game not found")) { }
    public GameNotFoundException(string message) : base(String.Format("Game not found: ", message)) { }
    public GameNotFoundException(string message, System.Exception inner) : base(String.Format("Game not found: ", message, inner)) { }

    protected GameNotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class GameNotFinishedException : System.Exception
{
    public GameNotFinishedException() : base(String.Format("Game not finished")) { }
    public GameNotFinishedException(string message) : base(String.Format("Game not finished", message)) { }
    public GameNotFinishedException(string message, System.Exception inner) : base(String.Format("Game not finished", message, inner)) { }

    protected GameNotFinishedException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

[Serializable()]
public class WrongValueException : System.Exception
{
    public WrongValueException() : base(String.Format("Wrong value")) { }
    public WrongValueException(string message) : base(String.Format("Wrong value", message)) { }
    public WrongValueException(string message, System.Exception inner) : base(String.Format("Wrong value", message, inner)) { }

    protected WrongValueException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
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
    public async Task<String> GetFreeFields(String id)
    {
        //etsitään oikea pelaaja
        var filter = Builders<Game>.Filter.Empty; //filtteri kaikille pelaajille
        var lista = await _gamesCollection.Find(filter).ToListAsync();

        foreach (var game in lista)
        {
            for (int i = 0; i < game._players.Count; i++)
            {
                if (game._players[i].Id == id) //id:tä vastaava pelaaja löydetty
                {
                    var _scoreboard = game._players[i].scoreboard.scores;
                    List<Combination> freeFields = new List<Combination>();
                    for (int j = 0; j < _scoreboard.Length; j++)
                    {
                        if (_scoreboard[j] == -1 && j != 7) // 7 on bonus
                        {
                            freeFields.Add((Combination)j);
                        }
                    }
                    return PrintFields(freeFields);
                }
            }
        }
        return null; //pelaajaa ei löytynyt;
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
                        throw new ScoreOutOfBoundsException(); // tähän voisi vaihtaa oman exceptionin 
                    }

                }
            }
        }
        return null;
    }

    public async Task<Player> DeleteScore(String id, Combination combination)
    {
        var filter = Builders<Game>.Filter.Empty;
        var lista = await _gamesCollection.Find(filter).ToListAsync();
        foreach (var game in lista)
        {
            for (int i = 0; i < game._players.Count; i++)
            {
                if (game._players[i].Id == id)
                {
                    Game newGame = new Game();
                    var filter2 = Builders<Game>.Filter.Eq(g => g.Id, game.Id);
                    newGame = await _gamesCollection.Find(filter2).FirstAsync();
                    Player newPlayer = game._players[i]; //pelajaa jonka id on sama kuin parametrinä saatu id
                    if ((int)combination == (int)Combination.total_up || (int)combination == (int)Combination.total)
                    {
                        newPlayer.scoreboard.scores[(int)combination] = 0; //total-kenttien alkuarvo on 0

                    }
                    else
                    {
                        if ((int)combination < 6) //yläkerrasta poistettaessa tuloksia, vähennetään molemmista totaleista
                        {
                            newPlayer.scoreboard.scores[(int)Combination.total_up] -= newPlayer.scoreboard.scores[(int)combination];
                            newPlayer.scoreboard.scores[(int)Combination.total] -= newPlayer.scoreboard.scores[(int)combination];
                        }
                        else //jos ei yläkerta, niin vain alakerran totalista vähennetään pisteitä
                        {
                            newPlayer.scoreboard.scores[(int)Combination.total] -= newPlayer.scoreboard.scores[(int)combination];
                        }
                        newPlayer.scoreboard.scores[(int)combination] = -1; //muiden kenttien alkuarvo on -1
                    }
                    newGame._players[i] = newPlayer;
                    //await CheckForBonus(id);
                    await _gamesCollection.ReplaceOneAsync(filter2, newGame);
                    return newPlayer;
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
        //Tein gamenotfoundexceptionin mut en oo ihan varma mitä tässä on ajettu takaa joten en lähteny sörkkimään t. ville

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
                    throw new GameNotFinishedException(); //tähän vois tehdä oman exceptionin
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
            if (score % (combination + 1) != 0 && score != 0)
            {
                throw new WrongValueException(); // tähän vois tehdä oman exceptionin
            }
        }
        if (combination == 8) // pari = mahd scoret 2 4 6 8 10 12
        {
            if (score % 2 != 0 || score > 12 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 9) // 2*pari = mahd scoret 6 8 10 12 14 16 18 20 22
        {
            if (score % 2 != 0 || score > 22 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 10) // kolmiluku = mahd pisteet 3 6 9 12 15 18
        {
            if (score % 3 != 0 || score > 18 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 11) // neliluku = mahd pisteet 4 8 12 16 20 24  
        {
            if (score % 4 != 0 || score > 24 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 12) // täyskäsi 
        {
            int[] pariScoret = { 2, 4, 6, 8, 10, 12 };
            int[] kolmilukuScoret = { 3, 6, 9, 12, 15, 18 };
            List<int> tayskasiScoret = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i != j) // esim 1+1 ja 1+1+1 ei ole täyskäsi, tai 2+2 ja 2+2+2 jne
                    {
                        tayskasiScoret.Add(pariScoret[i] + kolmilukuScoret[j]);
                    }
                }
            }
            if (!tayskasiScoret.Contains(score))
            {
                throw new WrongValueException("wrong value(full house)");
            }

        }
        if (combination == 13) // pieni suora on aina 15 pistettä (tai 0)
        {
            if (score != 15 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 14) // iso suora on aina 20 pistettä (tai 0)
        {
            if (score != 20 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 15) // sattuma, viidellä nopalla mahdollisimman suuri tulos, min 5 max 30
        {
            if (score < 5 || score > 30 && score != 0)
            {
                throw new WrongValueException();
            }
        }
        if (combination == 16) // yatzy, aina 50 tai 0 pistettä
        {
            if (score != 50 && score != 0)
            {
                throw new WrongValueException();
            }
        }
    }
    public async Task<String> GetScoreboard(String id)
    {
        var player = await GetPlayer(id);
        String playerScoreboard = File.ReadAllText("scoreboard.txt");
        String header = $"Scoreboard for {player.name}";
        for (int x = 0; x < 18; x++)
        {
            String value = " ";
            if (player.scoreboard.scores[x] != -1)
            {
                value = player.scoreboard.scores[x].ToString();
            }
            if (x < 10)
            {
                playerScoreboard = playerScoreboard.Replace($"_SCORE_0{x}", value);
            }
            else
            {
                playerScoreboard = playerScoreboard.Replace($"_SCORE_{x}", value);
            }
        }
        return String.Join("\n", header, playerScoreboard);
    }

    public async Task<Player> AddRandomValuesAll(String id)
    {
        var player = await GetPlayer(id);

        for (int i = 0; i < 17; i++)
        {
            Random rnd = new Random();
            if (i < 6) //yläkerta
            {
                int dice = i + 1; // index starts from 0
                int value = dice * rnd.Next(1, 6); //1-5 dice
                await AddScore(id, value, (Combination)i);
            }
            if (i == 8) // pari = mahd scoret 2 4 6 8 10 12
            {
                int[] scores = { 2, 4, 6, 8, 10, 12 };
                int value = scores[rnd.Next(0, scores.Length)];
                await AddScore(id, value, (Combination)i);
            }
            if (i == 9) // 2*pari = mahd scoret 6 8 10 12 14 16 18 20 22
            {
                int[] scores = { 6, 8, 10, 12, 14, 16, 18, 20, 22 };
                int value = scores[rnd.Next(0, scores.Length)];
                await AddScore(id, value, (Combination)i);
            }
            if (i == 10) // kolmiluku = mahd pisteet 3 6 9 12 15 18
            {
                int[] scores = { 3, 6, 9, 12, 15, 18 };
                int value = scores[rnd.Next(0, scores.Length)];
                await AddScore(id, value, (Combination)i);
            }
            if (i == 11) // neliluku = mahd pisteet 4,8,12,16,20,24
            {
                int[] scores = { 4, 8, 12, 16, 20, 24 };
                int value = scores[rnd.Next(0, scores.Length)];
                await AddScore(id, value, (Combination)i);
            }
            if (i == 12) //täyskäsi, kolmiluku + pari
            {
                int[] pariScoret = { 2, 4, 6, 8, 10, 12 };
                int[] kolmilukuScoret = { 3, 6, 9, 12, 15, 18 };
                int parIndex = rnd.Next(0, pariScoret.Length);
                int threeIndex = rnd.Next(0, kolmilukuScoret.Length);
                int returnValue = 0;
                if (parIndex == threeIndex) // jos tulos on 1+1 ja 1+1+1, asetetaan tulokseksi 0, (1+1+1+1+1 ei käy täyskäteen)
                {
                    returnValue = 0;
                }
                else
                {
                    returnValue = pariScoret[parIndex] + kolmilukuScoret[threeIndex];
                }
                await AddScore(id, returnValue, (Combination)i);
            }
            if (i == 13) // pieni suora aina 0/15p
            {
                int roll = rnd.Next(0, 100);
                if (roll < 50)
                {
                    await AddScore(id, 15, (Combination)i);
                }
                else
                {
                    await AddScore(id, 0, (Combination)i);
                }
            }
            if (i == 14) //iso suora aina 0/20p
            {
                int roll = rnd.Next(0, 100);
                if (roll < 50)
                {
                    await AddScore(id, 20, (Combination)i);
                }
                else
                {
                    await AddScore(id, 0, (Combination)i);
                }
            }
            if (i == 15) // sattuma, 5-30p;
            {
                await AddScore(id, rnd.Next(5, 31), (Combination)i);
            }
            if (i == 16)

            {
                int roll = rnd.Next(0, 100);
                if (roll < 34)
                {
                    await AddScore(id, 50, (Combination)i);
                }
                else
                {
                    await AddScore(id, 0, (Combination)i);
                }
            }
        }

        return player;
    }
    public async Task<Player> ResetFields(String id)
    {
        var player = await GetPlayer(id);

        for (int i = 0; i < 18; i++)
        {
            await DeleteScore(id, (Combination)i);
        }

        return player;
    }
    public String PrintFields(List<Combination> fieldList)
    {
        String header = "Available fields:";
        String content = "";
        foreach (var item in fieldList)
        {
            content += $"{(int)item}:{Enum.GetName(typeof(Combination), item)}\n";
        }
        if (content == "")
        {
            return "No available fields";
        }
        return String.Join("\n", header, content);
    }
    public async Task<Player> CheckForBonus(String id)
    {
        //pitää muistaa ylläpitää mongodb arvoja
        var filter = Builders<Game>.Filter.Empty;
        var lista = await _gamesCollection.Find(filter).ToListAsync();
        foreach (var game in lista)
        {
            for (int i = 0; i < game._players.Count; i++)
            {
                if (game._players[i].Id == id)
                {
                    Game newGame = new Game();
                    var filter2 = Builders<Game>.Filter.Eq(g => g.Id, game.Id);
                    newGame = await _gamesCollection.Find(filter2).FirstAsync();
                    Player newPlayer = game._players[i]; //pelajaa jonka id on sama kuin parametrinä saatu id
                    if (newPlayer.scoreboard.scores[(int)Combination.total_up] >= 63)
                    {
                        newPlayer.scoreboard.scores[(int)Combination.bonus] = 50;
                    }
                    else
                    {
                        newPlayer.scoreboard.scores[(int)Combination.bonus] = -1;
                    }
                    newGame._players[i] = newPlayer;
                    await _gamesCollection.ReplaceOneAsync(filter2, newGame);
                    return newPlayer;
                }
            }
        }
        return null; //player not found
    }
}