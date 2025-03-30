using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services
{
    public class ProjectService(IProjectRepository projectRepository) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<ServiceResult> CreateAsync(ProjectRegistrationForm form)
        {
            if (form == null)
                return ServiceResult.BadRequest();

            if (await projectRepository.ExistsAsync(p => p.ProjectName == form.ProjectName))
                return ServiceResult.AlreadyExists();

            try
            {
                var projectEntity = ProjectFactory.Map(form);
                var result = await _projectRepository.AddAsync(projectEntity);
                if (!result)
                    return ServiceResult.Failed();

                // Skickar tillbaka ett true (Succeeded)
                return ServiceResult.Created("Project created");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed(ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(ProjectUpdateForm form)
        {
            if (form == null)
                return ServiceResult.BadRequest();

            if (await _projectRepository.ExistsAsync(c => c.Id == form.Id))
            {
                try
                {
                    var projectToUpdate = await _projectRepository.GetAsync(p => p.Id == form.Id);
                    var projectEntity = ProjectFactory.Map(form, projectToUpdate);

                    var result = await _projectRepository.UpdateAsync(projectEntity);
                    if (!result)
                        return ServiceResult.Failed();

                    // Skickar tillbaka Updated (Succeeded)
                    return ServiceResult.Updated("Project Updated");
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

            if (await _projectRepository.ExistsAsync(p => p.Id == Id))
            {
                try
                {
                    var projectEntity = await _projectRepository.GetAsync(p => p.Id == Id);

                    var result = await _projectRepository.RemoveAsync(projectEntity);
                    if (!result)
                        return ServiceResult.Failed();

                    return ServiceResult.Ok("Project deleted");
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

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            var entites = await _projectRepository.GetAllAsync(
                orderByDescending: true,
                sortBy: x => x.Created,
                filterBy: null,
                i => i.Client,
                i => i.Status
            );

            var projects = entites.Select(ProjectFactory.Map);
            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            var entity = await _projectRepository.GetAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }
            return ProjectFactory.Map(entity);
        }

        public async Task<Project?> GetProjectByNameAsync(string name)
        {
            var entity = await _projectRepository.GetAsync(p => p.ProjectName == name);
            return ProjectFactory.Map(entity);
        }
    }
}