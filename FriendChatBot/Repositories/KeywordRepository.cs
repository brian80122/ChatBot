using FriendChatBot.Models.Entities;
using MongoDB.Driver;

namespace FriendChatBot.Repositories
{
    public class KeywordRepository : BaseRepository
    {
        public override IMongoDatabase GetDatabase()
        {
            return base.GetDatabase("chat");
        }

        public IMongoCollection<Keyword> GetCollection()
        {
            var db = GetDatabase();
            return db.GetCollection<Keyword>("keyword");
        }

        public bool Insert(Keyword entity)
        {
            var collection = GetCollection();
            var filter = Builders<Keyword>.Filter.Eq(c => c.Key, entity.Key);
            var found = collection.Find(filter).FirstOrDefault();
            if (found == null)
            {
                collection.InsertOne(entity);
            }
            else
            {
                var update = Builders<Keyword>.Update.Set(c => c.Message, entity.Message);
                collection.UpdateOne(filter, update);
            }

            return true;
        }

        public Keyword QueryByKey(string key)
        {
            return GetCollection().Find(c => c.Key == key).FirstOrDefault();
        }

        public bool Delete(string key)
        {
            return GetCollection().DeleteOne(c=>c.Key == key).DeletedCount > 0;
        }
    }
}
