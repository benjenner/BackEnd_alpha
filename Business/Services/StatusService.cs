using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;

namespace Business.Services
{
    public class StatusService(IStatusRepository statusRepository) : IStatusService
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