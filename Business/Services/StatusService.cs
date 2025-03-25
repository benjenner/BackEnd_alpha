using Business.Factories;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;

// Här sker mappning så vi kan hämta ut satliga statusdelar.
// Ska generera dropdown med värden
namespace Business.Services
{
    // Jobbar mot interface i konstruktorn
    public class StatusService(IStatusRepository statusRepository)
    {
        private readonly IStatusRepository _statusRepository = statusRepository;

        public async Task<IEnumerable<Status>> GetProjectStatuses()
        {
            var list = await _statusRepository.GetAllAsync(

                selector: x => StatusFactory.Map(x)!

                );
            return list;
        }
    }
}