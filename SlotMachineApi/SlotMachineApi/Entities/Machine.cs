using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SlotMachineApi.Entities
{
    public class Machine
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int SlotsSize { get; set; }
    }
}
