using System;
using System.Threading.Tasks;
using MongoDB.Bson;

using MongoDB.Driver;

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

    public Task<Player> AddScore(Guid id, int score, Combination combination)
    {
        throw new NotImplementedException();
    }

    public async Task<Player> CreateAPlayer(Player player, String id)
    {
        var filter = Builders<Game>.Filter.Eq(g => g.Id, id);
        Game foundGame = await _gamesCollection.Find(filter).FirstAsync();
        foundGame._players.Add(player);
        await _gamesCollection.ReplaceOneAsync(filter, foundGame);

        return player;
    }

    public Task<Player> GetPlayer(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetScore(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Player> GetWinner(Guid id_game)
    {
        throw new NotImplementedException();
    }

    public async Task<Game> StartNewGame(Game game)
    {
        await _gamesCollection.InsertOneAsync(game);
        return game;
    }
}