using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SlotMachineApi.DbSettings;
using SlotMachineApi.Entities;

namespace SlotMachineApi.Services.Impl
{
    public class MachineService :  IMachineService
    {
        private readonly IMongoCollection<Machine> _machineCollection;

        public MachineService(
          IOptions<SlotMachineDatabaseSettings> slotMachineDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                slotMachineDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                slotMachineDatabaseSettings.Value.DatabaseName);

            _machineCollection = mongoDatabase.GetCollection<Machine>(
            slotMachineDatabaseSettings.Value.MachinesCollectionName);
        }

        public int[] ReturnSlotsArray(Machine machine)
        {
            Random randNum = new Random();

            int[] slotsArray = Enumerable
                .Repeat(0, machine.SlotsSize)
                .Select(i => randNum.Next(0, 9))
                .ToArray();

            return slotsArray;
        }

        public async Task RefreshArray(string id, int newSize)
        {
            var machine = await GetById(id);

            if(newSize < 0)
            {
                throw new ArgumentOutOfRangeException("Size cant be < 0");
            }

            machine.SlotsSize = newSize;

            await _machineCollection.ReplaceOneAsync(x=> x.Id.Equals(machine.Id), machine);
        }

        public async Task<Machine> GetById(string id)
        {
            var result = await _machineCollection
                    .Find(x => x.Id.Equals(id))
                    .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Machine>> GetAllAsync()
        {
            var result = await _machineCollection.Find( _ => true).ToListAsync();

            return result;
        }

        public async Task Create(Machine machine)
        {
            if (machine == null || machine.SlotsSize < 0)
                throw new ArgumentException("Wrong machine data");

            await _machineCollection.InsertOneAsync(machine);
        }
    }
}
