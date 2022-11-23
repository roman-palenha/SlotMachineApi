using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using SlotMachineApi.DbSettings;
using SlotMachineApi.Entities;

namespace SlotMachineApi.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        private readonly IMongoCollection<Player> _playersCollection;


        //method bet, refresh arr
        //endpoint for change game  arr
        //user balance
        //update 
        //game change to machine
        public PlayerService(
           IOptions<SlotMachineDatabaseSettings> slotMachineDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                slotMachineDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                slotMachineDatabaseSettings.Value.DatabaseName);

            _playersCollection = mongoDatabase.GetCollection<Player>(
            slotMachineDatabaseSettings.Value.PlayersCollectionName);
        }

        public async Task CreateAsync(Player player)
        {
            if (GetByNameAsync(player.UserName) != null)
                throw new ArgumentException("There is a user with same username");

            await _playersCollection.InsertOneAsync(player);
        }

        public async Task DeleteAsync(Player player)
        {
            if (GetByNameAsync(player.UserName) == null)
                throw new ArgumentException("There is not a user with same username");

            await _playersCollection.DeleteOneAsync(x => x.UserName.ToLower().Equals(player.UserName.ToLower()));
        }

        public async Task<Player> GetByNameAsync(string username)
        {
            var result = await _playersCollection
                    .Find(x => x.UserName.ToLower().Equals(username.ToLower()))
                    .FirstOrDefaultAsync();
            return result;
        }

        public async Task UpdateBalanceAsync(string username, double balance)
        {
            var player = await GetByNameAsync(username);
            if (player == null)
                throw new ArgumentException("There is not a user with same username");

            player.Balance = balance;
            
            await _playersCollection.ReplaceOneAsync(x => x.UserName.ToLower().Equals(username.ToLower()), player);
        }
    }
}
