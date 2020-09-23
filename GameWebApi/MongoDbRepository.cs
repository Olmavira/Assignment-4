using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Permissions;
using MongoDB.Driver;
using MongoDB.Bson;
public class MongoDpRepository : IRepository
{
    string path = @"E:\Koulu\Periodi v3p1\Taustajarjestelmat\Assignment3\GameWebApi\game-dev.txt";
    List<Player> playerList = new List<Player>();

    private readonly IMongoCollection<Player> _playerCollection;
    private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

    public MongoDpRepository()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("game");
        _playerCollection = database.GetCollection<Player>("players");
        _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
    }

    public Task<Player> Get(Guid id)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
        return _playerCollection.Find(filter).FirstAsync();
    }
    public async Task<Player[]> GetAll()
    {
        // string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        // Player[] players = JsonConvert.DeserializeObject<Player[]>(jsonToBeDeserialized);

        var sortDef = Builders<Player>.Sort.Descending(player => player.Score);
        var players = await _playerCollection.Find(new BsonDocument()).Sort(sortDef).ToListAsync();
        return players.ToArray();

        //return Task.FromResult<Player[]>(players);
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
    public async Task<Player> Delete(Guid id)
    {
        FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(player => player.Id, id);
        return await _playerCollection.FindOneAndDeleteAsync(filter);
    }

    public Task<Item> CreateItem(Guid playerId, Item item)
    {
        var newItem = new Item
        {
            Id = item.Id,
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

        if (foundPlayer.Id == playerId)
        {
            Item[] playerItems = new Item[foundPlayer.Items.Length + 1];
            int count = 0;
            foreach (Item i in foundPlayer.Items)
            {
                playerItems[count] = foundPlayer.Items[count];
                count++;
            }
            foundPlayer.Items = playerItems;
            foundPlayer.Items[playerItems.Length - 1] = item;
            string output = JsonConvert.SerializeObject(foundPlayer);
            File.AppendAllText(path, output);
            return Task.FromResult<Item>(newItem);
        }
        return Task.FromResult<Item>(newItem);
    }
    public Task<Item> GetItem(Guid playerId, Guid itemId)
    {
        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(jsonToBeDeserialized);
        Player foundPlayer = new Player();
        Item foundItem = new Item();
        foreach (Player player in players)
        {
            if (player.Id == playerId)
            {
                foreach (Item i in player.Items)
                {
                    if (i.Id == itemId)
                    {
                        foundItem = i;
                    }
                }
                return Task.FromResult<Item>(foundItem);
            }
        }
        foundPlayer.Name = "not Found";
        return Task.FromResult<Item>(foundItem);
        //throw new NotImplementedException();
    }
    public Task<Item[]> GetAllItems(Guid playerId)
    {
        string jsonToBeDeserialized = System.IO.File.ReadAllText(path);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(jsonToBeDeserialized);
        Player foundPlayer = new Player();
        Item[] foundItems = new Item[0];
        foreach (Player player in players)
        {
            if (player.Id == playerId)
            {
                foundItems = player.Items;
                return Task.FromResult<Item[]>(foundItems);
            }
        }
        foundPlayer.Name = "not Found";
        return Task.FromResult<Item[]>(foundItems);
        //throw new NotImplementedExce
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


