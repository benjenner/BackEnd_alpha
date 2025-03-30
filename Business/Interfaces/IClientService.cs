using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ServiceResult> CreateAsync(ClientRegistrationForm form);

        Task<IEnumerable<Client?>> GetAllClientsAsync();

        Task<Client?> GetClientByIdAsync(int id);

        Task<Client?> GetClientByNameAsync(string name);

        Task<ServiceResult> RemoveAsync(int? Id);

        Task<ServiceResult> UpdateAsync(ClientUpdateForm form);
    }
}