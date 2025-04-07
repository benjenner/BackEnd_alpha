using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services
{
    public class StatusService(IStatusRepository statusRepository, IMemoryCache cache) : IStatusService
    {
        private readonly IStatusRepository _statusRepository = statusRepository;
        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "Status_All";

        public async Task<IEnumerable<Status>> GetProjectStatuses()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Status>? cachedItems))
            {
                return cachedItems;
            }

            _cache.Remove(_cacheKey_All);
            var entities = await _statusRepository.GetAllAsync();

            var statuses = entities.Select(StatusFactory.Map);
            _cache.Set(_cacheKey_All, statuses, TimeSpan.FromMinutes(10));
            return statuses;
        }
    }
}