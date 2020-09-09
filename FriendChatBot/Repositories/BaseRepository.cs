using System.Linq;
using MongoDB.Driver;

namespace FriendChatBot.Repositories
{
    public class BaseRepository
    {
        public MongoClient GetClient()
        {
            return new MongoClient(""); ;
        }

        public virtual IMongoDatabase GetDatabase()
        {
          return GetDatabase("");
        }

        public virtual IMongoDatabase GetDatabase(string databaseName)
        {
            var client = GetClient();
            return client.GetDatabase(databaseName);
        }
    }
}
