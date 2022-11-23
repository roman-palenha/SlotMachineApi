using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SlotMachineApi.DbSettings;
using SlotMachineApi.Entities;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace SlotMachineApi.Services.Impl
{
    public class GameService: IGameService
    {
        private readonly IMongoCollection<Game> _gameCollection;

        public GameService(
          IOptions<SlotMachineDatabaseSettings> slotMachineDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                slotMachineDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                slotMachineDatabaseSettings.Value.DatabaseName);

            _gameCollection = mongoDatabase.GetCollection<Game>(
            slotMachineDatabaseSettings.Value.GamesCollectionName);
        }
        public int[] ReturnSlotsArray(Game game)
        {
            Random randNum = new Random();

            int[] slotsArray = Enumerable
                .Repeat(0, game.SlotsSize)
                .Select(i => randNum.Next(0, 9))
                .ToArray();
            return slotsArray;
        }

        public async Task Update(Game game, int newSize)
        {
            game.SlotsSize = newSize;

            await _gameCollection.ReplaceOneAsync(x=> x.Id.Equals(game.Id), game);
        }
    }
}
