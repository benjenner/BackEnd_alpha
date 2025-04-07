using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, IMemoryCache cache) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IMemoryCache _cache = cache;
    private const string _cacheKey_All = "Client_All";

    public async Task<ServiceResult> CreateAsync(ClientRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        if (await _clientRepository.ExistsAsync(c => c.ClientName == form.ClientName))
            return ServiceResult.AlreadyExists();

        try
        {
            var clientEntity = ClientFactory.Map(form);
            var result = await _clientRepository.AddAsync(clientEntity);
            if (!result)
            {
                return ServiceResult.Failed();
            }
            _cache.Remove(_cacheKey_All);

            // Skickar tillbaka ett true (Succeeded)
            return ServiceResult.Created("Client created");
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }

    public async Task<ServiceResult> UpdateAsync(ClientUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        if (await _clientRepository.ExistsAsync(c => c.Id == form.Id))
        {
            try
            {
                var clientToUpdate = await _clientRepository.GetAsync(c => c.Id == form.Id);
                var clientEntity = ClientFactory.Map(form, clientToUpdate);

                var result = await _clientRepository.UpdateAsync(clientEntity);
                if (!result)
                {
                    return ServiceResult.Failed();
                }

                _cache.Remove(_cacheKey_All);
                // Skickar tillbaka Updated (Succeeded)
                return ServiceResult.Updated("Client Updated");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed(ex.Message);
            }
        }
        else
        {
            return ServiceResult.NotFound();
        }
    }

    public async Task<ServiceResult> RemoveAsync(int? Id)
    {
        if (Id == null)
            return ServiceResult.BadRequest();

        if (await _clientRepository.ExistsAsync(c => c.Id == Id))
        {
            try
            {
                var clientEntity = await _clientRepository.GetAsync(c => c.Id == Id);

                var result = await _clientRepository.RemoveAsync(clientEntity);
                if (!result)
                {
                    return ServiceResult.Failed();
                }

                _cache.Remove(_cacheKey_All);
                return ServiceResult.Ok("Client deleted");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed(ex.Message);
            }
        }
        else
        {
            return ServiceResult.NotFound();
        }
    }

    public async Task<IEnumerable<Client?>> GetAllClientsAsync()
    {
        if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Client>? cachedItems))
        {
            return cachedItems;
        }

        _cache.Remove(_cacheKey_All);
        var entities = await _clientRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.ClientName,
            filterBy: null,

            i => i.ClientContactInformation,
            i => i.Address,
            i => i.Projects
        );

        var clients = entities.Select(ClientFactory.Map);
        _cache.Set(_cacheKey_All, clients, TimeSpan.FromMinutes(10));

        return clients;
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Client>? cachedItems))
        {
            Client client = cachedItems?.FirstOrDefault(x => x.Id == id);
            if (client != null)
                return client;
        }

        _cache.Remove(_cacheKey_All);
        var entities = await _clientRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.ClientName,
            filterBy: null,

            i => i.ClientContactInformation,
            i => i.Address,
            i => i.Projects
        );

        var clients = entities.Select(ClientFactory.Map);
        _cache.Set(_cacheKey_All, clients, TimeSpan.FromMinutes(10));

        return clients.SingleOrDefault(x => x.Id == id);
    }

    public async Task<Client?> GetClientByNameAsync(string name)
    {
        if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Client>? cachedItems))
        {
            Client client = cachedItems?.FirstOrDefault(x => x.ClientName.ToLower() == name.ToLower());
            if (client != null)
                return client;
        }

        _cache.Remove(_cacheKey_All);
        var entities = await _clientRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.ClientName,
            filterBy: null,

            i => i.ClientContactInformation,
            i => i.Address,
            i => i.Projects
        );

        var clients = entities.Select(ClientFactory.Map);
        _cache.Set(_cacheKey_All, clients, TimeSpan.FromMinutes(10));

        return clients.SingleOrDefault(x => x.ClientName.ToLower() == name.ToLower());
    }
}