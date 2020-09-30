using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FileRepository : IRepository
{
    static PlayersList playersList;
    public async Task<Player> Create(Player player)
    {
        string path = @"D:\PeliKehitysBACK\Pelikehitys_taustajarj\GameWebApi\game-dev.txt";
        if (!File.Exists(path))
        {
            //File.WriteAllText(path, JsonConvert.SerializeObject(player));
            await Task.Run(() => File.WriteAllText(path, JsonConvert.SerializeObject(player)));
        }
        return player;
    }

    public Task<Player> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Player> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Player[]> GetAll()
    {
        string playersString;
        string path = @"D:\PeliKehitysBACK\Pelikehitys_taustajarj\GameWebApi\game-dev.txt";

        playersString = File.ReadAllText(path);
        playersList = JsonConvert.DeserializeObject<PlayersList>(playersString);

        return playersList.players.ToArray();
    }

    public Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
        throw new NotImplementedException();
    }
}

public class PlayersList
{
    public List<Player> players { get; set; }
}