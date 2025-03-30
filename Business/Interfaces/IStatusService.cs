using Domain.Models;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetProjectStatuses();
    }
}