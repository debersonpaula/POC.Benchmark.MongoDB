using DataApi.Repository;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DataApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonCollectionRepository _personCollectionRepository;
        private readonly PersonDocumentRepository _personDocumentRepository;
        private readonly ILogger<PersonController> logger;

        public PersonController(PersonCollectionRepository personCollectionRepository, PersonDocumentRepository personDocumentRepository, ILogger<PersonController> logger)
        {
            _personCollectionRepository = personCollectionRepository;
            _personDocumentRepository = personDocumentRepository;
            this.logger = logger;
        }

        [HttpGet("collection/{id}")]
        public Task<PersonModel> GetPersonByCollection(int id)
        {
            logger.LogInformation("{@method}: id = {@id}", nameof(GetPersonByCollection), id);
            return _personCollectionRepository.GetPersonById(id);
        }

        [HttpGet("document/{id}")]
        public Task<PersonModel> GetPersonByDocument(int id)
        {
            logger.LogInformation("{@method}: id = {@id}", nameof(GetPersonByDocument), id);
            return _personDocumentRepository.GetPersonById(id);
        }
    }
}
