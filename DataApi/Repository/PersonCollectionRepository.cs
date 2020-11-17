using DataModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace DataApi.Repository
{
    public class PersonCollectionRepository : IPersonRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public PersonCollectionRepository()
        {
            _client = new MongoClient("mongodb://user:user@localhost:27017/DB_BY_COLLECTIONS");
            _database = _client.GetDatabase("DB_BY_COLLECTIONS");
        }

        public async Task<PersonModel> GetPersonById(int id)
        {
            var collection = GetCollection<PersonModel>("Persons_id" + id.ToString());
            var person = await collection.Find(new BsonDocument()).ToListAsync();
            return person.FirstOrDefault();
        }

        private IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
        {
            return _database.GetCollection<TDocument>(name, settings);
        }
    }
}
