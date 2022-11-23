using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SlotMachineApi.Entities
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public double Balance { get; set; }
   }
}
