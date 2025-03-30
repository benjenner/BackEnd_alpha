using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult> CreateAsync(ProjectRegistrationForm form);

        Task<Project?> GetProjectByIdAsync(int id);

        Task<IEnumerable<Project>> GetProjectsAsync();

        Task<ServiceResult> RemoveAsync(int? Id);

        Task<ServiceResult> UpdateAsync(ProjectUpdateForm form);

        Task<Project?> GetProjectByNameAsync(string name);
    }
}