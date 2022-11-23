using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SlotMachineApi.DbSettings;
using SlotMachineApi.Entities;
using System;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace SlotMachineApi.Services.Impl
{
    public class MachineService :  IMachineService

    {
        private readonly IMongoCollection<Machine> _gameCollection;

        public MachineService(
          IOptions<SlotMachineDatabaseSettings> slotMachineDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                slotMachineDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                slotMachineDatabaseSettings.Value.DatabaseName);

            _gameCollection = mongoDatabase.GetCollection<Machine>(
            slotMachineDatabaseSettings.Value.MachinesCollectionName);
        }
        public int[] ReturnSlotsArray(Machine game)
        {
            Random randNum = new Random();

            int[] slotsArray = Enumerable
                .Repeat(0, game.SlotsSize)
                .Select(i => randNum.Next(0, 9))
                .ToArray();
            return slotsArray;
        }

        public async Task RefreshArray(string id, int newSize)
        {
            var game = await GetById(id);

            game.SlotsSize = newSize;

            await _gameCollection.ReplaceOneAsync(x=> x.Id.Equals(game.Id), game);
        }
        public async Task<Machine> GetById(string id)
        {
            var result = await _gameCollection
                    .Find(x => x.Id.Equals(id))
                    .FirstOrDefaultAsync();
            return result;
        }
        public async Task<Machine> Create(Machine game)
        {
            await _gameCollection.InsertOneAsync(game);
            return game;
        }
    }
}
