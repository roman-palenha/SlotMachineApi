using Microsoft.AspNetCore.Mvc;
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
            if (await GetByNameAsync(player.UserName) != null)
                throw new ArgumentException("There is a user with same username");

            if (player.Balance < 0)
                throw new ArgumentException("Cannot be a user with negative balance");

            await _playersCollection.InsertOneAsync(player);
        }

        public async Task DeleteAsync(Player player)
        {
            if (GetByNameAsync(player.UserName) == null)
                throw new ArgumentException("There is not a user with same username");

            await _playersCollection.DeleteOneAsync(x => x.UserName.ToLower().Equals(player.UserName.ToLower()));
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var result = await _playersCollection.Find(_ => true).ToListAsync();

            return result;
        }

        public async Task<Player?> GetByNameAsync(string username)
        {
            var foundUsers = await _playersCollection.FindAsync(x => x.UserName.Equals(username));
            var result = await foundUsers.FirstOrDefaultAsync();

            return result;
        }

        public async Task UpdateBalanceAsync(string username, double balance)
        {
            var player = await GetByNameAsync(username);
            if (player == null)
                throw new ArgumentException("There is not a user with same username");

            if(balance < 0)
                throw new ArgumentException("Negative balance");

            player.Balance = balance;
            
            await _playersCollection.ReplaceOneAsync(x => x.UserName.Equals(username), player);
        }
    }
}
