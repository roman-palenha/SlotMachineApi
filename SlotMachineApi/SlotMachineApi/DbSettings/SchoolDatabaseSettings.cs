namespace SlotMachineApi.DbSettings
{
    public class SlotMachineDatabaseSettings : ISlotMachineDatabaseSettings
    {
        public string PlayersCollectionName { get; set; }
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ISlotMachineDatabaseSettings
    {
        public string PlayersCollectionName { get; set; }
        public string GamesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
