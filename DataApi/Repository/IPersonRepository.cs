using DataModels;
using System.Threading.Tasks;

namespace DataApi.Repository
{
    public interface IPersonRepository
    {
        Task<PersonModel> GetPersonById(int id);
    }
}