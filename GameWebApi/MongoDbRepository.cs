using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Permissions;
public class MongoDpRepository : IRepository
{
    string path = @"E:\Koulu\Periodi v3p1\Taustajarjestelmat\Assignment3\GameWebApi\game-dev.txt";
    List<Player> playerList = new List<Player>();

    public Task<Player> Get(Guid id)
    {
        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(jsonToBeDeserialized);
        Player foundPlayer = new Player();
        foreach (Player player in players)
        {
            if (player.Id == id)
            {
                foundPlayer = player;
                return Task.FromResult<Player>(foundPlayer);
            }

        }
        foundPlayer.Name = "not Found";
        return Task.FromResult<Player>(foundPlayer);
    }
    public Task<Player[]> GetAll()
    {
        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        Player[] players = JsonConvert.DeserializeObject<Player[]>(jsonToBeDeserialized);

        return Task.FromResult<Player[]>(players);
    }
    public Task<Player> Create(Player player)
    {
        var newlycreatedPlayer = new Player
        {
            Id = player.Id,
            Name = player.Name,
            Score = 0,
            Level = 0,
            IsBanned = false,
            CreationTime = DateTime.Now
        };

        string output = JsonConvert.SerializeObject(newlycreatedPlayer);
        File.AppendAllText(path, output);

        return Task.FromResult<Player>(newlycreatedPlayer);
    }
    public Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(jsonToBeDeserialized);
        Player foundPlayer = new Player();
        foreach (Player playeri in players)
        {
            if (playeri.Id == id)
            {   //foundPlayer
                playeri.Score = player.Score;
                string output = JsonConvert.SerializeObject(players);
                File.WriteAllText(path, output);
                return Task.FromResult<Player>(playeri);
            }
        }
        foundPlayer.Name = "not Found";
        return Task.FromResult<Player>(foundPlayer);
    }
    public Task<Player> Delete(Guid id)
    {
        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(jsonToBeDeserialized);
        Player foundPlayer = new Player();
        foreach (Player playeri in players)
        {
            if (playeri.Id == id)
            {
                foundPlayer = playeri;
                players.Remove(playeri);
                string output = JsonConvert.SerializeObject(players);
                File.WriteAllText(path, output);
                return Task.FromResult<Player>(foundPlayer);
            }

        }
        foundPlayer.Name = "not Found";
        return Task.FromResult<Player>(foundPlayer);
    }

    public Task<Item> CreateItem(Guid playerId, Item item)
    {
        var newItem = new Item
        {
            Level = 0,
            Type = 0,
            CreationTime = DateTime.Now
        };

        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(jsonToBeDeserialized);
        Player foundPlayer = new Player();
        foreach (Player player in players)
        {
            if (player.Id == playerId)
            {
                foundPlayer = player;
            }

        }
        foundPlayer.Name = "not Found";


        if (foundPlayer.Id == playerId)
        {
            foundPlayer.Items.Add(item);
            string output = JsonConvert.SerializeObject(foundPlayer);
            File.AppendAllText(path, output);
            return Task.FromResult<Item>(newItem);
        }
        return Task.FromResult<Item>(newItem);
    }
    public Task<Item> GetItem(Guid playerId, Guid itemId)
    {
        throw new NotImplementedException();
    }
    public Task<Item[]> GetAllItems(Guid playerId)
    {
        throw new NotImplementedException();
    }
    public Task<Item> UpdateItem(Guid playerId, Item item)
    {
        throw new NotImplementedException();
    }
    public Task<Item> DeleteItem(Guid playerId, Item item)
    {
        throw new NotImplementedException();
    }

}


