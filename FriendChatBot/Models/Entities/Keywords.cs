using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FriendChatBot.Models.Entities
{
    public class Keyword
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
    }
}
