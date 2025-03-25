using Business.Factories;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

// Jobbar mot interfaces i konstruktorn
public class ClientService(IClientRepository clientRepository,
    IClientContactInformationRepository clientInformationRepository,
    IClientAddressRepository clientAddressRepository)
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IClientContactInformationRepository _clientInformationRepository = clientInformationRepository;
    private readonly IClientAddressRepository _clientAddressRepository = clientAddressRepository;

    public async Task<ServiceResult> CreateAsync(ClientRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest();

        if (await _clientRepository.ExistsAsync(x => x.ClientName == form.ClientName))
            return ServiceResult.AlreadyExists();

        try
        {
            // ***** Skapa upp med factory istället********
            var clientEntity = new ClientEntity();
            var result = await _clientRepository.AddAsync(clientEntity);
            if (!result)
                return ServiceResult.Failed();

            // Skapar kontaktinformation
            await _clientInformationRepository.AddAsync(clientEntity.ClientContactInformation);

            // Skapar adress
            await _clientAddressRepository.AddAsync(clientEntity.Address);

            // Skickar tillbaka ett true (Succeeded)
            return ServiceResult.Created();
        }
        catch (Exception ex)
        {
            return ServiceResult.Failed(ex.Message);
        }
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        // Hämtar samtliga client-entiteter
        var list = await _clientRepository.GetAllAsync();
        // listan med entiteter filtreras ut enligt Client-modellen i ClientFactory.
        var clients = list.Select(ClientFactory.Map);

        // Skickar tillbaka listan. Kan/ska man istället returnera ServiceResult?
        // Här kan man specificera hur listan ska returneras (return list.OrderBy())

        return clients;
    }
}