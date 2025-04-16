using Authentication.Entities;
using Business.Factories;
using Business.Interfaces;
using Data.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services
{
    public class ProjectService(IProjectRepository projectRepository, IMemoryCache cache, IAzureFileHandler filehandler) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IAzureFileHandler _filehandler = filehandler;

        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "Project_All";

        public async Task<ServiceResult> CreateAsync(ProjectRegistrationForm form)
        {
            if (form == null)
                return ServiceResult.BadRequest();

            if (await projectRepository.ExistsAsync(p => p.ProjectName == form.ProjectName))
                return ServiceResult.AlreadyExists();

            try
            {
                var projectEntity = ProjectFactory.Map(form);

                var fileName = await _filehandler.UploadFileAsync(form.Image);
                projectEntity.Image = fileName;

                var result = await _projectRepository.AddAsync(projectEntity);
                if (!result)
                {
                    return ServiceResult.Failed();
                }

                _cache.Remove(_cacheKey_All);
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

                    var fileName = await _filehandler.UploadFileAsync(form.NewImage);
                    projectEntity.Image = fileName;

                    var result = await _projectRepository.UpdateAsync(projectEntity);
                    if (!result)
                    {
                        return ServiceResult.Failed();
                    }

                    _cache.Remove(_cacheKey_All);

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
            {
                return ServiceResult.BadRequest();
            }

            if (await _projectRepository.ExistsAsync(p => p.Id == Id))
            {
                try
                {
                    var projectEntity = await _projectRepository.GetAsync(p => p.Id == Id);

                    var result = await _projectRepository.RemoveAsync(projectEntity);
                    if (!result)
                    {
                        return ServiceResult.Failed();
                    }

                    _cache.Remove(_cacheKey_All);
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
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Project>? cachedItems))
            {
                return cachedItems;
            }

            _cache.Remove(_cacheKey_All);
            var entities = await _projectRepository.GetAllAsync(
               orderByDescending: true,
               sortBy: x => x.Created,
               filterBy: null,
                i => i.Client,
                i => i.Client.Address,
                i => i.Client.ClientContactInformation,
                i => i.Status
           );

            var projects = entities.Select(ProjectFactory.Map);
            _cache.Set(_cacheKey_All, projects, TimeSpan.FromMinutes(10));

            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Project>? cachedItems))
            {
                Project project = cachedItems?.FirstOrDefault(x => x.Id == id);
                if (project != null)
                    return project;
            }

            _cache.Remove(_cacheKey_All);
            var entities = await _projectRepository.GetAllAsync(
               orderByDescending: true,
               sortBy: x => x.Created,
               filterBy: null,
                i => i.Client,
                i => i.Client.Address,
                i => i.Client.ClientContactInformation,
                i => i.Status
           );

            var projects = entities.Select(ProjectFactory.Map);
            _cache.Set(_cacheKey_All, projects, TimeSpan.FromMinutes(10));

            return projects.SingleOrDefault(x => x.Id == id);
        }

        public async Task<Project?> GetProjectByNameAsync(string name)
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<Project>? cachedItems))
            {
                Project project = cachedItems?.FirstOrDefault(x => x.ProjectName.ToLower() == name.ToLower());
                if (project != null)
                    return project;
            }

            _cache.Remove(_cacheKey_All);
            var entities = await _projectRepository.GetAllAsync(
               orderByDescending: true,
               sortBy: x => x.Created,
               filterBy: null,
                i => i.Client,
                i => i.Client.Address,
                i => i.Client.ClientContactInformation,
                i => i.Status
           );

            var projects = entities.Select(ProjectFactory.Map);
            _cache.Set(_cacheKey_All, projects, TimeSpan.FromMinutes(10));

            return projects.SingleOrDefault(x => x.ProjectName.ToLower() == name.ToLower());
        }
    }
}