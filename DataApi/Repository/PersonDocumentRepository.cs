using DataModels;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace DataApi.Repository
{
    public class PersonDocumentRepository : IPersonRepository
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public PersonDocumentRepository()
        {
            _client = new MongoClient("mongodb://user:user@localhost:27018/DB_BY_DOCUMENTS");
            _database = _client.GetDatabase("DB_BY_DOCUMENTS");
        }

        public async Task<PersonModel> GetPersonById(int id)
        {
            var collection = GetCollection<PersonModel>("Persons");
            var filter = Builders<PersonModel>.Filter.Eq("PersonId", id);
            var person = await collection.FindAsync(filter);
            return person.FirstOrDefault();
        }

        private IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
        {
            return _database.GetCollection<TDocument>(name, settings);
        }
    }

    class GetPersonByIdFilter
    {
        public GetPersonByIdFilter(int personId)
        {
            PersonId = personId;
        }

        public int PersonId { get; set; }
    }
}
