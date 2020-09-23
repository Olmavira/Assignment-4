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

namespace GameWebApi
{
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
            var filter = Builders<Player>.Filter.Empty;
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();
            return players.ToArray();

            //return Task.FromResult<Player[]>(players);
        }
        public async Task<Player> Create(Player player)
        {
            await _playerCollection.InsertOneAsync(player);
            return player;
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

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();

            if (player == null)
            {
                throw new NotFoundException("Player Not Found!");
            }
            if (player.Items == null)
            {
                player.Items = new List<Item>();

                player.Items.Add(item);
                return item;
            }
            return item;
        }
        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player player = await Get(playerId);
            //var filter = Builders<Item>.Filter.Eq(item => item.Id, itemId);

            for (int i = 0; i < player.Items.Count; i++)
            {
                if (player.Items[i].Id == itemId)
                    return player.Items[i];
            }

            return null;
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            return player.Items.ToArray();
        }
        public async Task<Item> UpdateItem(Guid playerId, Guid itemId, ModifiedItem item)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();

            for (int i = 0; i < player.Items.Count; i++)
            {
                if (player.Items[i].Id == itemId)
                {
                    player.Items[i].Level = item.Level;
                    await _playerCollection.ReplaceOneAsync(filter, player);
                    return player.Items[i];
                }

            }
            return null;
        }
        public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            Player player = await _playerCollection.Find(filter).FirstAsync();
            Item itemToRemove = null;

            for (int x = 0; x < player.Items.Count; x++)
            {
                if (player.Items[x].Id == itemId)
                {
                    itemToRemove = player.Items[x];
                    player.Items.RemoveAt(x);
                    await _playerCollection.ReplaceOneAsync(filter, player);
                    return itemToRemove;
                }
            }
            return itemToRemove;
        }
    }
}


