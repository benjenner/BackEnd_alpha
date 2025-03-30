using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

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
                return ServiceResult.Failed();

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
                    return ServiceResult.Failed();

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
                    return ServiceResult.Failed();

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

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        var entity = await _clientRepository.GetAsync(x => x.Id == id);
        if (entity == null)
        {
            return null;
        }
        return ClientFactory.Map(entity);
    }

    public async Task<Client?> GetClientByNameAsync(string name)
    {
        var entity = await _clientRepository.GetAsync(x => x.ClientName == name);
        return ClientFactory.Map(entity);
    }

    public async Task<IEnumerable<Client?>> GetAllClientsAsync()
    {
        var list = await _clientRepository.GetAllAsync();

        if (list == null)
        {
            return null;
        }
        var clients = list.Select(ClientFactory.Map);

        return clients;
    }
}